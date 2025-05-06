namespace TenantInspectAdmin.API.ConsoleLogger
{
    public class ColorConsoleLoggerConfiguration
    {
        public int EventId { get; set; }
        public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new()
        {
            [LogLevel.Information] = ConsoleColor.Green,
        };
    }
}
