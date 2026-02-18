using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VOCENuOrderApp.Data;

namespace VOCENuOrderApp.Services
{
    public class LogCleanupService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LogCleanupService> _logger;

        public LogCleanupService(ApplicationDbContext context, ILogger<LogCleanupService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CleanOldLogsAsync()
        {
            var enabled = await _context.AppSettings
                .AsNoTracking()
                .Where(s => s.Key == "AutoCleanLogs")
                .Select(s => s.Value)
                .FirstOrDefaultAsync();

            if (!string.Equals(enabled, "true", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Auto-cleanup is disabled. Skipping.");
                return;
            }

            var daysStr = await _context.AppSettings
                .AsNoTracking()
                .Where(s => s.Key == "AutoCleanLogsDays")
                .Select(s => s.Value)
                .FirstOrDefaultAsync();

            int retentionDays = int.TryParse(daysStr, out var d) && d >= 1 ? d : 14;
            var cutoff = DateTime.UtcNow.AddDays(-retentionDays);

            var replenDeleted = await _context.ReplenishmentSyncLogs
                .Where(l => l.Timestamp < cutoff)
                .ExecuteDeleteAsync();

            var productDeleted = await _context.ProductSyncLogs
                .Where(l => l.Timestamp < cutoff)
                .ExecuteDeleteAsync();

            _logger.LogInformation(
                "Log cleanup complete: {ReplenCount} replenishment + {ProductCount} product logs older than {Days} days removed.",
                replenDeleted, productDeleted, retentionDays);
        }
    }
}
