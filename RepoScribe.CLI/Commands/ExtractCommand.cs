using System.CommandLine;
using RepoScribe.Core.Services;
using RepoScribe.Core.Utilities;
using Serilog;
using System.Threading;
using System.CommandLine.Invocation;
using System.CommandLine.IO;

namespace RepoScribe.CLI.Commands
{
    public class ExtractCommand : BaseCommand
    {
        public override Command GetCommand()
        {
            var command = new Command("extract", "Extract code blocks from Markdown files");

            var configPathOption = new Option<string>(
                new[] { "--config", "-c" },
                description: "Path to the configuration file",
                getDefaultValue: () => "config.json");

            command.AddOption(configPathOption);

            // Assign the handler using a lambda that accepts the config option and the InvocationContext
            command.SetHandler((string config, InvocationContext context) =>
                HandleExtract(config, context),
                configPathOption);

            return command;
        }

        private void HandleExtract(string config, InvocationContext context)
        {
            var console = context.Console;
            var cancellationToken = context.GetCancellationToken();

            try
            {
                var configManager = new ConfigurationManager(config);
                var inputDirectory = configManager.GetExtractChunksInputDirectory();

                if (string.IsNullOrEmpty(inputDirectory) || !Directory.Exists(inputDirectory))
                {
                    Log.Error($"Invalid input directory: {inputDirectory}");
                    context.ExitCode = 1;
                    return;
                }

                var extractor = new MarkdownExtractor(inputDirectory);
                var codeBlocks = extractor.ExtractCodeBlocks();

                foreach (var codeBlock in codeBlocks)
                {
                    console.Out.WriteLine(codeBlock);
                }

                Log.Information("Code blocks extraction completed.");
                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to extract code blocks.");
                context.ExitCode = 1;
            }
            finally
            {
                Logger.CloseAndFlush();
            }
        }
    }
}
