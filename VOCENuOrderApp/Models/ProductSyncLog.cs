using System.ComponentModel.DataAnnotations;

namespace VOCENuOrderApp.Models
{
    public class ProductSyncLog
    {
        public int Id { get; set; }

        [Required]
        public string BasePartNumber { get; set; }

        [Required]
        public string ColorName { get; set; }

        [Required]
        public string SyncStatus { get; set; } // "Success" / "Failed"

        public string Message { get; set; }

        public string Brand { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Optional: raw request/response payloads for diagnostics
        public string? RequestJson { get; set; }
        public string? ResponseJson { get; set; }
    }
}
