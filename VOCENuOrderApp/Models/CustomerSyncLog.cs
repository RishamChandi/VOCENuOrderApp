using System.ComponentModel.DataAnnotations;

namespace VOCENuOrderApp.Models
{
    public class CustomerSyncLog
    {
        public int Id { get; set; }

        [Required]
        public string CustomerCode { get; set; } // Xoro customer Id or code

        public string CustomerName { get; set; }

        [Required]
        public string Client { get; set; } // e.g., VOCE

        [Required]
        public string SyncStatus { get; set; } // Success / Failed

        public string Message { get; set; } // error or info

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
