using System.ComponentModel.DataAnnotations;

namespace VOCENuOrderApp.Models
{
    public class SyncCursor
    {
        [Key]
        [MaxLength(100)]
        public string Key { get; set; } = string.Empty;
        public DateTime LastRunUtc { get; set; }
    }
}
