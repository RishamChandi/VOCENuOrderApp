using System;
using System.ComponentModel.DataAnnotations;

namespace VOCENuOrderApp.Models.NUORDER
{
    public class OrderStatusSyncLog
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string OrderNumber { get; set; } = string.Empty; // NuOrder order_number
        [MaxLength(100)]
        public string NuOrderInternalId { get; set; } = string.Empty; // optional if known
        [Required, MaxLength(50)]
        public string PreviousStatus { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string TargetStatus { get; set; } = string.Empty;
        public bool Success { get; set; }
        [MaxLength(50)]
        public string ErrorCode { get; set; } = string.Empty;
        [MaxLength(4000)]
        public string ErrorMessage { get; set; } = string.Empty;
        [MaxLength(50)]
        public string EndpointUsed { get; set; } = string.Empty; // path or body endpoint
        [MaxLength(50)]
        public string Client { get; set; } = string.Empty; // VOCE
        // Diagnostics
        [MaxLength(1000)]
        public string RequestUrl { get; set; } = string.Empty;
        [MaxLength(4000)]
        public string RequestJson { get; set; } = string.Empty;
        [MaxLength(4000)]
        public string ResponseBody { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ResponseStatusCode { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
