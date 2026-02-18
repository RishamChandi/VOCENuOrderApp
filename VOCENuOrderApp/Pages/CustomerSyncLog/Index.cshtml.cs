using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VOCENuOrderApp.Data;
using VOCENuOrderApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace VOCENuOrderApp.Pages.CustomerSyncLog
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<VOCENuOrderApp.Models.CustomerSyncLog> Logs { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? FilterClient { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? FilterCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? FilterStatus { get; set; }

        public async Task OnGetAsync()
        {
            var query = _db.CustomerSyncLogs.AsNoTracking().OrderByDescending(x => x.Timestamp).AsQueryable();

            if (!string.IsNullOrWhiteSpace(FilterClient))
                query = query.Where(x => x.Client == FilterClient);
            if (!string.IsNullOrWhiteSpace(FilterCode))
                query = query.Where(x => x.CustomerCode == FilterCode);
            if (!string.IsNullOrWhiteSpace(FilterStatus))
                query = query.Where(x => x.SyncStatus == FilterStatus);

            Logs = await query.Take(500).ToListAsync();
        }
    }
}
