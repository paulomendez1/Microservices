using System.ComponentModel.DataAnnotations;

namespace CommandService.DTOs
{
    public class CommandCreateDTO
    {
        [Required]
        public string HowTo { get; set; } = string.Empty!;
    [Required]
        public string CommandLine { get; set; } = string.Empty!;
    }
}
