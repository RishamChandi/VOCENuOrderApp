using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VOCENuOrderApp.Models.NUORDER;

namespace VOCENuOrderApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly NuOrderVOCEConfig _nuOrderConfig;

        public SettingsController(IConfiguration config, IOptions<NuOrderVOCEConfig> nuOrderConfig)
        {
            _config = config;
            _nuOrderConfig = nuOrderConfig.Value;
        }

        public IActionResult Index()
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

            return View();
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
                if (trimmed.StartsWith("Server=", StringComparison.OrdinalIgnoreCase) ||
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
