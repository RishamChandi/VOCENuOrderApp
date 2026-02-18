using System.ComponentModel.DataAnnotations;

namespace VOCENuOrderApp.Models
{
    public class AppSetting
    {
        [Key]
        [MaxLength(128)]
        public string Key { get; set; } = null!;

        [MaxLength(512)]
        public string? Value { get; set; }
    }
}
