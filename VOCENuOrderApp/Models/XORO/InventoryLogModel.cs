namespace VOCENuOrderApp.Models.XORO
{
    public class InventoryLogModel
    {
        // Job Log Fields
        public int Id { get; set; }
        public string? JobName { get; set; }
        public string? JobStatus { get; set; }
        public string? JobMessage { get; set; }
        public string? JobJson { get; set; }
        public string? JobResultJson { get; set; }
        public bool? JobResult { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
