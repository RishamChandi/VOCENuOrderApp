using System;

namespace VOCENuOrderApp.Models.NUORDER
{
    public class ReplenishmentSyncLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string BasePartNumber { get; set; }
        public string Color { get; set; }
        public string? Size { get; set; }
        public string? InventoryType { get; set; }
        public string SkuId { get; set; }
        public float? ATS { get; set; }
        public float? POQty { get; set; }
        public bool Prebook { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string? Client { get; set; }
        public string? RequestJson { get; set; }
        public string? ResponseJson { get; set; }
    }
}
