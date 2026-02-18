using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VOCENuOrderApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOCENuOrderApp.Views.ReplenishmentSyncLog
{
    [Obsolete("This file has been moved to Pages/ReplenishmentSyncLog. Razor Pages should not be in Views.")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<global::VOCENuOrderApp.Models.NUORDER.ReplenishmentSyncLog> Logs { get; set; } = new();
        public string StatusFilter { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string StyleFilter { get; set; }
        public string ColorFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(string statusFilter, string fromDate, string toDate, string styleFilter, string colorFilter, int? pageNumber)
        {
            StatusFilter = statusFilter;
            FromDate = fromDate;
            ToDate = toDate;
            StyleFilter = styleFilter;
            ColorFilter = colorFilter;
            PageNumber = pageNumber ?? 1;
            var logs = _context.ReplenishmentSyncLogs.AsQueryable();
            if (!string.IsNullOrEmpty(statusFilter))
                logs = logs.Where(l => l.Status == statusFilter);
            if (DateTime.TryParse(fromDate, out var fromDt))
                logs = logs.Where(l => l.Timestamp >= fromDt);
            if (DateTime.TryParse(toDate, out var toDt))
                logs = logs.Where(l => l.Timestamp <= toDt);
            if (!string.IsNullOrEmpty(styleFilter))
                logs = logs.Where(l => l.BasePartNumber.Contains(styleFilter));
            if (!string.IsNullOrEmpty(colorFilter))
                logs = logs.Where(l => l.Color.Contains(colorFilter));
            TotalCount = await logs.CountAsync();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Logs = await logs.OrderByDescending(l => l.Timestamp)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        public string GetQueryString(int page)
        {
            var query = new Dictionary<string, string?>
            {
                ["statusFilter"] = StatusFilter,
                ["fromDate"] = FromDate,
                ["toDate"] = ToDate,
                ["styleFilter"] = StyleFilter,
                ["colorFilter"] = ColorFilter,
                ["pageNumber"] = page.ToString()
            };
            return string.Join("&", query.Where(kv => !string.IsNullOrEmpty(kv.Value)).Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value!)}"));
        }
    }
}
