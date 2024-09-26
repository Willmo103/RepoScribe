using System.CommandLine;
using System.CommandLine.Invocation;
using RepoScribe.Core.Services;
using Serilog;

namespace RepoScribe.CLI.Commands
{
    public class ExtractCommand
    {
        public Command GetCommand()
        {
            var command = new Command("extract", "Extract code blocks from Markdown files");

            var configPathOption = new Option<string>(
                new[] { "--config", "-c" },
                description: "Path to the configuration file",
                getDefaultValue: () => "config.json");

            command.AddOption(configPathOption);

            command.Handler = CommandHandler.Create<string>(HandleExtract);

            return command;
        }

        private void HandleExtract(string config)
        {
            try
            {
                var configManager = new ConfigurationManager(config);
                var inputDirectory = configManager.GetExtractChunksInputDirectory();

                if (string.IsNullOrEmpty(inputDirectory) || !Directory.Exists(inputDirectory))
                {
                    Log.Error($"Invalid input directory: {inputDirectory}");
                    return;
                }

                var extractor = new MarkdownExtractor(inputDirectory);
                var codeBlocks = extractor.ExtractCodeBlocks();

                foreach (var codeBlock in codeBlocks)
                {
                    Console.WriteLine(codeBlock);
                }

                Log.Information("Code blocks extraction completed.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to extract code blocks.");
            }
        }
    }
}
