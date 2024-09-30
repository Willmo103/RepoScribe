using Serilog;

namespace RepoScribe.Core.Utilities
{
    public static class Logger
    {
        public static void Initialize()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }
    }
}
