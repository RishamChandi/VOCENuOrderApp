using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VOCENuOrderApp.Data;
using VOCENuOrderApp.Models;
using VOCENuOrderApp.Models.NUORDER;

namespace VOCENuOrderApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly NuOrderVOCEConfig _nuOrderConfig;
        private readonly ApplicationDbContext _context;

        public SettingsController(IConfiguration config, IOptions<NuOrderVOCEConfig> nuOrderConfig, ApplicationDbContext context)
        {
            _config = config;
            _nuOrderConfig = nuOrderConfig.Value;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var connStr = _config.GetConnectionString("DefaultConnection") ?? "";
            var serverName = ExtractServer(connStr);

            var jobs = new List<HangfireJobInfo>();
            try
            {
                using var connection = JobStorage.Current.GetConnection();
                foreach (var dto in connection.GetRecurringJobs())
                {
                    jobs.Add(new HangfireJobInfo
                    {
                        Id = dto.Id,
                        Cron = dto.Cron,
                        LastExecution = dto.LastExecution,
                        NextExecution = dto.NextExecution
                    });
                }
            }
            catch { }

            ViewBag.NuOrderBaseUrl = "https://next.nuorder.com";
            ViewBag.NuOrderConsumerKey = Mask(_nuOrderConfig.ConsumerKey);
            ViewBag.NuOrderToken = Mask(_nuOrderConfig.Token);

            ViewBag.XoroBaseUrl = "https://res.xorosoft.io";
            ViewBag.XoroClientId = Mask(_config["XoroOptions:ClientId"] ?? "bab2705e4cbd419291bfaa03bfe7e7cc");

            ViewBag.DbServer = serverName;
            ViewBag.DbName = ExtractDbName(connStr);

            ViewBag.HangfireJobs = jobs;

            var autoClean = await GetSetting("AutoCleanLogs");
            ViewBag.AutoCleanLogs = string.Equals(autoClean, "true", StringComparison.OrdinalIgnoreCase);

            var daysVal = await GetSetting("AutoCleanLogsDays");
            ViewBag.AutoCleanLogsDays = int.TryParse(daysVal, out var days) ? days : 14;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAutoClean(bool enabled, int days)
        {
            if (days < 1) days = 14;
            if (days > 365) days = 365;

            await UpsertSetting("AutoCleanLogs", enabled ? "true" : "false");
            await UpsertSetting("AutoCleanLogsDays", days.ToString());

            TempData["SettingsMessage"] = enabled
                ? $"Auto-cleanup enabled. Logs older than {days} days will be removed daily."
                : "Auto-cleanup disabled.";

            return RedirectToAction("Index");
        }

        private async Task<string?> GetSetting(string key)
        {
            return await _context.AppSettings
                .AsNoTracking()
                .Where(s => s.Key == key)
                .Select(s => s.Value)
                .FirstOrDefaultAsync();
        }

        private async Task UpsertSetting(string key, string value)
        {
            var setting = await _context.AppSettings.FindAsync(key);
            if (setting == null)
            {
                _context.AppSettings.Add(new AppSetting { Key = key, Value = value });
            }
            else
            {
                setting.Value = value;
            }
            await _context.SaveChangesAsync();
        }

        private static string Mask(string? value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 8) return "****";
            return value[..4] + new string('*', value.Length - 8) + value[^4..];
        }

        private static string ExtractServer(string connStr)
        {
            foreach (var part in connStr.Split(';'))
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("Host=", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.StartsWith("Server=", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed.Split('=', 2)[1].Trim();
                }
            }
            return "Unknown";
        }

        private static string ExtractDbName(string connStr)
        {
            foreach (var part in connStr.Split(';'))
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("Database=", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.StartsWith("Initial Catalog=", StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed.Split('=', 2)[1].Trim();
                }
            }
            return "Unknown";
        }
    }

    public class HangfireJobInfo
    {
        public string Id { get; set; } = "";
        public string Cron { get; set; } = "";
        public DateTime? LastExecution { get; set; }
        public DateTime? NextExecution { get; set; }
    }
}
