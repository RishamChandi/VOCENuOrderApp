using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VOCENuOrderApp.Data;
using System.Text;
using System.Linq;

public class ProductSyncLogController : Controller
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 20;

    public ProductSyncLogController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string statusFilter, string basePartFilter, string colorFilter, string brandFilter, DateTime? fromDate, DateTime? toDate, int page = 1)
    {
        _context.Database.SetCommandTimeout(120);
        var logs = _context.ProductSyncLogs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(statusFilter))
            logs = logs.Where(l => l.SyncStatus == statusFilter);
        if (!string.IsNullOrEmpty(basePartFilter))
            logs = logs.Where(l => l.BasePartNumber != null && l.BasePartNumber.StartsWith(basePartFilter));
        if (!string.IsNullOrEmpty(colorFilter))
            logs = logs.Where(l => l.ColorName != null && l.ColorName.StartsWith(colorFilter));
        if (!string.IsNullOrEmpty(brandFilter))
            logs = logs.Where(l => l.Brand == brandFilter);
        // Apply default date window if none provided (last 7 days) to avoid full scans
        if (!fromDate.HasValue && !toDate.HasValue)
        {
            var today = DateTime.UtcNow.Date;
            fromDate = today.AddDays(-7);
            toDate = today.AddDays(1).AddTicks(-1);
        }
        if (fromDate.HasValue)
            logs = logs.Where(l => l.Timestamp >= fromDate.Value);
        if (toDate.HasValue)
            logs = logs.Where(l => l.Timestamp <= toDate.Value);

        // Get unique brands for dropdown from the same filtered set to avoid scanning the entire table
        var brands = await logs
            .Where(l => !string.IsNullOrEmpty(l.Brand))
            .Select(l => l.Brand)
            .Distinct()
            .OrderBy(b => b)
            .Take(200)
            .ToListAsync();
        ViewBag.BrandList = brands;

        var totalLogs = await logs.CountAsync(HttpContext.RequestAborted);
        var logPage = await logs
            .OrderByDescending(l => l.Timestamp)
            .ThenByDescending(l => l.Id)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync(HttpContext.RequestAborted);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalLogs / PageSize);
        ViewBag.StatusFilter = statusFilter;
        ViewBag.BasePartFilter = basePartFilter;
        ViewBag.ColorFilter = colorFilter;
        ViewBag.BrandFilter = brandFilter;
        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

        return View(logPage);
    }

    // Server-side brand search to avoid loading entire brand list on initial page load
    [HttpGet]
    public async Task<IActionResult> BrandSearch(string term, int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return Json(Array.Empty<string>());
        }

        var brands = await _context.ProductSyncLogs
            .AsNoTracking()
            .Where(l => l.Brand != null && EF.Functions.Like(l.Brand, term + "%"))
            .Select(l => l.Brand)
            .Distinct()
            .OrderBy(b => b)
            .Take(Math.Clamp(limit, 1, 50))
            .ToListAsync(HttpContext.RequestAborted);

        return Json(brands);
    }

    public async Task<IActionResult> ExportToCsv(string statusFilter, string basePartFilter, string colorFilter, string brandFilter, DateTime? fromDate, DateTime? toDate)
    {
        var logs = _context.ProductSyncLogs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(statusFilter))
            logs = logs.Where(l => l.SyncStatus == statusFilter);
        if (!string.IsNullOrEmpty(basePartFilter))
            logs = logs.Where(l => l.BasePartNumber.StartsWith(basePartFilter));
        if (!string.IsNullOrEmpty(colorFilter))
            logs = logs.Where(l => l.ColorName.StartsWith(colorFilter));
        if (!string.IsNullOrEmpty(brandFilter))
            logs = logs.Where(l => l.Brand == brandFilter);
        if (!fromDate.HasValue && !toDate.HasValue)
        {
            var today = DateTime.UtcNow.Date;
            fromDate = today.AddDays(-7);
            toDate = today.AddDays(1).AddTicks(-1);
        }
        if (fromDate.HasValue)
            logs = logs.Where(l => l.Timestamp >= fromDate.Value);
        if (toDate.HasValue)
            logs = logs.Where(l => l.Timestamp <= toDate.Value);

        var logList = await logs
            .OrderByDescending(l => l.Timestamp)
            .ThenByDescending(l => l.Id)
            .ToListAsync(HttpContext.RequestAborted);

        var sb = new StringBuilder();
        sb.AppendLine("Timestamp,BasePartNumber,ColorName,SyncStatus,Brand,Message,RequestJson,ResponseJson");

        foreach (var log in logList)
        {
            string Trunc(string? s) => string.IsNullOrWhiteSpace(s) ? string.Empty : (s.Length <= 500 ? s : s.Substring(0, 500));
            string Esc(string? s) => (s ?? string.Empty).Replace("\"", "'");
            sb.AppendLine($"\"{log.Timestamp:yyyy-MM-dd HH:mm:ss}\",\"{log.BasePartNumber}\",\"{log.ColorName}\",\"{log.SyncStatus}\",\"{log.Brand}\",\"{Esc(log.Message)}\",\"{Esc(Trunc(log.RequestJson))}\",\"{Esc(Trunc(log.ResponseJson))}\"");
        }

        var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(csvBytes, "text/csv", "ProductSyncLogs.csv");
    }

    // Lazy-load full RequestJson
    [HttpGet]
    public async Task<IActionResult> GetRequestJson(int id)
    {
        var payload = await _context.ProductSyncLogs.AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => new { l.RequestJson })
            .FirstOrDefaultAsync();
        if (payload == null || string.IsNullOrWhiteSpace(payload.RequestJson)) return NotFound();
        return Content(payload.RequestJson, "application/json");
    }

    // Lazy-load full ResponseJson
    [HttpGet]
    public async Task<IActionResult> GetResponseJson(int id)
    {
        var payload = await _context.ProductSyncLogs.AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => new { l.ResponseJson })
            .FirstOrDefaultAsync();
        if (payload == null || string.IsNullOrWhiteSpace(payload.ResponseJson)) return NotFound();
        return Content(payload.ResponseJson, "application/json");
    }
}
