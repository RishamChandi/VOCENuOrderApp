using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VOCENuOrderApp.Data;
using VOCENuOrderApp.Models;
using System.Diagnostics;

namespace VOCENuOrderApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var todayUtc = DateTime.UtcNow.Date;

            var productStats = await _db.ProductSyncLogs
                .Where(l => l.Timestamp >= todayUtc)
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Success = g.Count(l => l.SyncStatus == "Success"),
                    Failed = g.Count(l => l.SyncStatus != "Success")
                })
                .FirstOrDefaultAsync();

            var replenStats = await _db.ReplenishmentSyncLogs
                .Where(l => l.Timestamp >= todayUtc)
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Success = g.Count(l => l.Status == "Success"),
                    Failed = g.Count(l => l.Status != "Success")
                })
                .FirstOrDefaultAsync();

            var orderStats = await _db.OrderStatusSyncLogs
                .Where(l => l.Timestamp >= todayUtc)
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Success = g.Count(l => l.Success),
                    Failed = g.Count(l => !l.Success)
                })
                .FirstOrDefaultAsync();

            var customerTotal = await _db.CustomerSyncLogs
                .CountAsync(l => l.Timestamp >= todayUtc);

            ViewBag.ProductTotal = productStats?.Total ?? 0;
            ViewBag.ProductSuccess = productStats?.Success ?? 0;
            ViewBag.ProductFailed = productStats?.Failed ?? 0;

            ViewBag.ReplenTotal = replenStats?.Total ?? 0;
            ViewBag.ReplenSuccess = replenStats?.Success ?? 0;
            ViewBag.ReplenFailed = replenStats?.Failed ?? 0;

            ViewBag.OrderTotal = orderStats?.Total ?? 0;
            ViewBag.OrderSuccess = orderStats?.Success ?? 0;
            ViewBag.OrderFailed = orderStats?.Failed ?? 0;

            ViewBag.CustomerTotal = customerTotal;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
