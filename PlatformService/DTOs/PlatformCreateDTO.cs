using System.ComponentModel.DataAnnotations;

namespace PlatformService.DTOs
{
    public class PlatformCreateDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty!;
        [Required]
        public string Publisher { get; set; } = string.Empty!;
        [Required]
        public string Cost { get; set; } = string.Empty!;
    }
}
