using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VOCENuOrderApp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace VOCENuOrderApp.Controllers
{
    public class ReplenishmentSyncLogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 30;
        private const int MaxDaysRange = 31;

        public ReplenishmentSyncLogController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(string statusFilter, string fromDate, string toDate, string styleFilter, string colorFilter, int page = 1)
        {
            _context.Database.SetCommandTimeout(120);
            var logs = _context.ReplenishmentSyncLogs.AsNoTracking();

            DateTime? fromDt = null; DateTime? toDtVal = null;
            if (DateTime.TryParse(fromDate, out var f)) fromDt = f.Date;
            if (DateTime.TryParse(toDate, out var t)) toDtVal = t.Date.AddDays(1).AddTicks(-1);

            if (!fromDt.HasValue && !toDtVal.HasValue)
            {
                var today = DateTime.UtcNow.Date;
                fromDt = today.AddDays(-7);
                toDtVal = today.AddDays(1).AddTicks(-1);
            }
            if (fromDt.HasValue && toDtVal.HasValue && (toDtVal.Value - fromDt.Value).TotalDays > MaxDaysRange)
            {
                toDtVal = fromDt.Value.AddDays(MaxDaysRange);
            }

            if (!string.IsNullOrEmpty(statusFilter)) logs = logs.Where(l => l.Status == statusFilter);
            if (fromDt.HasValue) logs = logs.Where(l => l.Timestamp >= fromDt.Value);
            if (toDtVal.HasValue) logs = logs.Where(l => l.Timestamp <= toDtVal.Value);
            if (!string.IsNullOrEmpty(styleFilter)) logs = logs.Where(l => l.BasePartNumber != null && l.BasePartNumber.StartsWith(styleFilter));
            if (!string.IsNullOrEmpty(colorFilter)) logs = logs.Where(l => l.Color != null && l.Color.StartsWith(colorFilter));

            // Count with narrow projection
            var totalLogs = await logs.Select(l => l.Id).CountAsync(HttpContext.RequestAborted);

            // Project to a lightweight shape (exclude RequestJson/ResponseJson)
            var logPage = await logs
                .OrderByDescending(l => l.Timestamp)
                .ThenByDescending(l => l.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(l => new ReplenLogRow
                {
                    Id = l.Id,
                    Timestamp = l.Timestamp,
                    BasePartNumber = l.BasePartNumber,
                    Color = l.Color,
                    SkuId = l.SkuId,
                    ATS = l.ATS,
                    POQty = l.POQty,
                    Prebook = l.Prebook,
                    Status = l.Status,
                    Message = l.Message,
                    Client = l.Client
                })
                .ToListAsync(HttpContext.RequestAborted);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalLogs / PageSize);
            ViewBag.StatusFilter = statusFilter;
            ViewBag.FromDate = fromDt?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDtVal.HasValue ? toDtVal.Value.ToString("yyyy-MM-dd") : null;
            ViewBag.StyleFilter = styleFilter;
            ViewBag.ColorFilter = colorFilter;

            return View(logPage);
        }

        // Retain lazy JSON fetch endpoint
        [HttpGet]
        public async Task<IActionResult> GetRequestJson(int id)
        {
            var log = await _context.ReplenishmentSyncLogs.AsNoTracking()
                .Where(l => l.Id == id)
                .Select(l => new { l.RequestJson })
                .FirstOrDefaultAsync();
            if (log == null || string.IsNullOrWhiteSpace(log.RequestJson)) return NoContent();
            return Content(log.RequestJson, "application/json");
        }

        // NEW: lazy-load full ResponseJson for copy action
        [HttpGet]
        public async Task<IActionResult> GetResponseJson(int id)
        {
            var log = await _context.ReplenishmentSyncLogs.AsNoTracking()
                .Where(l => l.Id == id)
                .Select(l => new { l.ResponseJson })
                .FirstOrDefaultAsync();
            if (log == null || string.IsNullOrWhiteSpace(log.ResponseJson)) return NoContent();
            return Content(log.ResponseJson, "application/json");
        }

        public async Task<IActionResult> ExportCsv(string statusFilter, string fromDate, string toDate, string styleFilter, string colorFilter)
        {
            _context.Database.SetCommandTimeout(180);
            var logs = _context.ReplenishmentSyncLogs.AsNoTracking();
            DateTime? fromDt = null; DateTime? toDtVal = null;
            if (DateTime.TryParse(fromDate, out var f)) fromDt = f.Date;
            if (DateTime.TryParse(toDate, out var t)) toDtVal = t.Date.AddDays(1).AddTicks(-1);
            if (fromDt.HasValue && toDtVal.HasValue && (toDtVal.Value - fromDt.Value).TotalDays > MaxDaysRange)
                toDtVal = fromDt.Value.AddDays(MaxDaysRange);
            if (!string.IsNullOrEmpty(statusFilter)) logs = logs.Where(l => l.Status == statusFilter);
            if (fromDt.HasValue) logs = logs.Where(l => l.Timestamp >= fromDt.Value);
            if (toDtVal.HasValue) logs = logs.Where(l => l.Timestamp <= toDtVal.Value);
            if (!string.IsNullOrEmpty(styleFilter)) logs = logs.Where(l => l.BasePartNumber != null && l.BasePartNumber.StartsWith(styleFilter));
            if (!string.IsNullOrEmpty(colorFilter)) logs = logs.Where(l => l.Color != null && l.Color.StartsWith(colorFilter));

            var list = await logs
                .OrderByDescending(l => l.Timestamp)
                .ThenByDescending(l => l.Id)
                .ToListAsync(HttpContext.RequestAborted);

            var sb = new StringBuilder();
            sb.AppendLine("Timestamp,BasePart,Color,SkuId,ATS,POQty,Prebook,Status,Message,RequestJson,ResponseJson");
            foreach (var l in list)
            {
                sb.AppendLine($"\"{l.Timestamp:yyyy-MM-dd HH:mm:ss}\",\"{l.BasePartNumber}\",\"{l.Color}\",\"{l.SkuId}\",{l.ATS?.ToString() ?? ""},{l.POQty?.ToString() ?? ""},{l.Prebook},{l.Status},\"{(l.Message ?? "").Replace("\"","'")}\",\"{(Truncate(l.RequestJson,500)).Replace("\"","'")}\",\"{(Truncate(l.ResponseJson,500)).Replace("\"","'")}\"");
            }
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "ReplenishmentSyncLogs.csv");
        }

        private static string Truncate(string? s, int max) => string.IsNullOrWhiteSpace(s) ? string.Empty : (s.Length <= max ? s : s.Substring(0, max));

        // Thin row type for list view
        public class ReplenLogRow
        {
            public int Id { get; set; }
            public DateTime Timestamp { get; set; }
            public string BasePartNumber { get; set; }
            public string Color { get; set; }
            public string SkuId { get; set; }
            public float? ATS { get; set; }
            public float? POQty { get; set; }
            public bool Prebook { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }
            public string? Client { get; set; }
        }
    }
}
