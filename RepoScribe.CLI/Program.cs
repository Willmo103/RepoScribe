using System.CommandLine;
using RepoScribe.CLI.Commands;
using Serilog;

namespace RepoScribe.CLI
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // Initialize the logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var rootCommand = new RootCommand("RepoScribe - Flatten and document your code repositories");

            rootCommand.AddCommand(new FlattenCommand().GetCommand());
            rootCommand.AddCommand(new IgnoreCommand().GetCommand());
            rootCommand.AddCommand(new RepoCommand().GetCommand());
            rootCommand.AddCommand(new ExtractCommand().GetCommand());

            return await rootCommand.InvokeAsync(args);
        }
    }
}
