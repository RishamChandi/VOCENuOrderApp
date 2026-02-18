using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VOCENuOrderApp.Data;
using VOCENuOrderApp.Models.NUORDER;

namespace VOCENuOrderApp.Pages.Voce
{
    public class MainModel : PageModel
    {
        private const int PageSize = 30;
        private const int MaxDaysRange = 31;
        private readonly ApplicationDbContext _context;

        public MainModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public static IReadOnlyList<string> StatusOptions { get; } = new[] { "Success", "Failed", "Submitted" };

        public IReadOnlyList<VoceLogRow> Logs { get; private set; } = Array.Empty<VoceLogRow>();

        public int CurrentPage { get; private set; } = 1;

        public int TotalPages { get; private set; } = 1;

        public int TotalRecords { get; private set; } = 0;

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? StyleFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ColorFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [BindProperty(SupportsGet = true, Name = "page")]
        public int PageNumber { get; set; } = 1;

        public string? FromDateRouteValue => FromDate?.ToString("yyyy-MM-dd");

        public string? ToDateRouteValue => ToDate?.ToString("yyyy-MM-dd");

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var query = BuildFilteredQuery();

            TotalRecords = await query.CountAsync(cancellationToken);

            CurrentPage = PageNumber < 1 ? 1 : PageNumber;
            TotalPages = Math.Max(1, (int)Math.Ceiling(TotalRecords / (double)PageSize));
            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }

            var skip = (CurrentPage - 1) * PageSize;

            Logs = await query
                .OrderByDescending(l => l.Timestamp)
                .ThenByDescending(l => l.Id)
                .Skip(skip)
                .Take(PageSize)
                .Select(l => new VoceLogRow
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
                    Message = l.Message
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IActionResult> OnGetExportAsync(CancellationToken cancellationToken)
        {
            var query = BuildFilteredQuery();

            var rows = await query
                .OrderByDescending(l => l.Timestamp)
                .ThenByDescending(l => l.Id)
                .ToListAsync(cancellationToken);

            var sb = new StringBuilder();
            sb.AppendLine("Timestamp (UTC),Base Part,Color,SKU,ATS,PO Qty,Prebook,Status,Message,RequestJson,ResponseJson");

            foreach (var log in rows)
            {
                sb.Append('"').Append(log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")).Append("\",");
                sb.Append('"').Append(CsvEscape(log.BasePartNumber)).Append("\",");
                sb.Append('"').Append(CsvEscape(log.Color)).Append("\",");
                sb.Append('"').Append(CsvEscape(log.SkuId)).Append("\",");
                sb.Append('"').Append(log.ATS?.ToString("0.##") ?? string.Empty).Append("\",");
                sb.Append('"').Append(log.POQty?.ToString("0.##") ?? string.Empty).Append("\",");
                sb.Append('"').Append(log.Prebook ? "Yes" : "No").Append("\",");
                sb.Append('"').Append(CsvEscape(log.Status)).Append("\",");
                sb.Append('"').Append(CsvEscape(Truncate(log.Message))).Append("\",");
                sb.Append('"').Append(CsvEscape(Truncate(log.RequestJson))).Append("\",");
                sb.Append('"').Append(CsvEscape(Truncate(log.ResponseJson))).Append('"');
                sb.AppendLine();
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"voce-sync-logs-{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
        }

        public sealed class VoceLogRow
        {
            public int Id { get; init; }
            public DateTime Timestamp { get; init; }
            public string? BasePartNumber { get; init; }
            public string? Color { get; init; }
            public string? SkuId { get; init; }
            public float? ATS { get; init; }
            public float? POQty { get; init; }
            public bool Prebook { get; init; }
            public string Status { get; init; } = string.Empty;
            public string? Message { get; init; }
        }

        private IQueryable<ReplenishmentSyncLog> BuildFilteredQuery()
        {
            var query = _context.ReplenishmentSyncLogs
                .AsNoTracking()
                .Where(l => l.Client == "VOCE");

            var from = FromDate?.Date;
            var to = ToDate?.Date;

            if (!from.HasValue && !to.HasValue)
            {
                var todayUtc = DateTime.UtcNow.Date;
                from = todayUtc.AddDays(-7);
                to = todayUtc;
            }

            if (from.HasValue && to.HasValue && (to.Value - from.Value).TotalDays > MaxDaysRange)
            {
                to = from.Value.AddDays(MaxDaysRange);
            }

            FromDate = from;
            ToDate = to;

            if (!string.IsNullOrWhiteSpace(StatusFilter))
            {
                var trimmed = StatusFilter.Trim();
                query = query.Where(l => l.Status == trimmed);
            }

            if (from.HasValue)
            {
                query = query.Where(l => l.Timestamp >= from.Value);
            }

            if (to.HasValue)
            {
                var inclusiveTo = to.Value.AddDays(1).AddTicks(-1);
                query = query.Where(l => l.Timestamp <= inclusiveTo);
            }

            if (!string.IsNullOrWhiteSpace(StyleFilter))
            {
                var prefix = StyleFilter.Trim();
                query = query.Where(l => l.BasePartNumber != null && l.BasePartNumber.StartsWith(prefix));
            }

            if (!string.IsNullOrWhiteSpace(ColorFilter))
            {
                var prefix = ColorFilter.Trim();
                query = query.Where(l => l.Color != null && l.Color.StartsWith(prefix));
            }

            return query;
        }

        private static string CsvEscape(string? value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Replace("\"", "\"\"");
        }

        private static string Truncate(string? value, int max = 500)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            return value.Length <= max ? value : value[..max];
        }
    }
}
