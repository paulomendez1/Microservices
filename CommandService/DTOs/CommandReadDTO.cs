﻿namespace CommandService.DTOs
{
    public class CommandReadDTO
    {
        public int Id { get; set; }
        public string HowTo { get; set; } = string.Empty!;
        public string CommandLine { get; set; } = string.Empty!;
        public int PlatformId { get; set; }
    }
}
