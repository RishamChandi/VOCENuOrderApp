using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VOCENuOrderApp.Data;
using VOCENuOrderApp.Models.NUORDER;
using System.Text;

namespace VOCENuOrderApp.Controllers
{
    public class OrderStatusSyncLogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 50;
        public OrderStatusSyncLogController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(string orderNumber, string endpoint, string success, string targetStatus, DateTime? fromDate, DateTime? toDate, int page = 1)
        {
            var q = _context.OrderStatusSyncLogs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(orderNumber)) q = q.Where(l => l.OrderNumber.Contains(orderNumber));
            if (!string.IsNullOrWhiteSpace(endpoint)) q = q.Where(l => l.EndpointUsed.Contains(endpoint));
            if (!string.IsNullOrWhiteSpace(targetStatus)) q = q.Where(l => l.TargetStatus == targetStatus);
            if (!string.IsNullOrWhiteSpace(success))
            {
                if (bool.TryParse(success, out var sBool)) q = q.Where(l => l.Success == sBool);
            }
            if (fromDate.HasValue) q = q.Where(l => l.Timestamp >= fromDate.Value);
            if (toDate.HasValue) q = q.Where(l => l.Timestamp <= toDate.Value);

            var total = await q.CountAsync();
            var logs = await q.OrderByDescending(l => l.Timestamp)
                               .Skip((page - 1) * PageSize)
                               .Take(PageSize)
                               .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)PageSize);
            ViewBag.OrderNumber = orderNumber;
            ViewBag.Endpoint = endpoint;
            ViewBag.Success = success;
            ViewBag.TargetStatus = targetStatus;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(logs);
        }

        public async Task<IActionResult> ExportCsv(string orderNumber, string endpoint, string success, string targetStatus, DateTime? fromDate, DateTime? toDate)
        {
            var q = _context.OrderStatusSyncLogs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(orderNumber)) q = q.Where(l => l.OrderNumber.Contains(orderNumber));
            if (!string.IsNullOrWhiteSpace(endpoint)) q = q.Where(l => l.EndpointUsed.Contains(endpoint));
            if (!string.IsNullOrWhiteSpace(targetStatus)) q = q.Where(l => l.TargetStatus == targetStatus);
            if (!string.IsNullOrWhiteSpace(success) && bool.TryParse(success, out var sBool)) q = q.Where(l => l.Success == sBool);
            if (fromDate.HasValue) q = q.Where(l => l.Timestamp >= fromDate.Value);
            if (toDate.HasValue) q = q.Where(l => l.Timestamp <= toDate.Value);
            var data = await q.OrderByDescending(l => l.Timestamp).ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Timestamp,OrderNumber,InternalId,PreviousStatus,TargetStatus,Success,EndpointUsed,ErrorCode,ErrorMessage");
            foreach (var l in data)
            {
                sb.AppendLine($"{l.Timestamp:yyyy-MM-dd HH:mm:ss},{l.OrderNumber},{l.NuOrderInternalId},{l.PreviousStatus},{l.TargetStatus},{l.Success},{l.EndpointUsed},{l.ErrorCode},\"{l.ErrorMessage.Replace("\"","'")}\"");
            }
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "OrderStatusSyncLogs.csv");
        }
    }
}
