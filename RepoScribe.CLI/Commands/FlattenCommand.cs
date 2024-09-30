using System.CommandLine;
using RepoScribe.Core.DataModels.Markdown;
using RepoScribe.Core.Helpers;
using RepoScribe.Core.Utilities;
using Serilog;
using RepoScribe.Core.DataModels;
using RepoScribe.Core.FileHandlers;
using System.CommandLine.Invocation;

namespace RepoScribe.CLI.Commands
{
    public class FlattenCommand : BaseCommand
    {
        public override Command GetCommand()
        {
            var command = new Command("flatten", "Flatten a codebase into a Markdown document");

            var inputOption = new Option<string>(
                new[] { "--input", "-i" },
                description: "The root directory to process")
            { IsRequired = true };

            var outputOption = new Option<string>(
                new[] { "--output", "-o" },
                description: "The output Markdown file. Default: {input_basename}.md");

            var compressOption = new Option<bool>(
                new[] { "--compress", "-c" },
                getDefaultValue: () => false,
                description: "Enable content compression");

            command.AddOption(inputOption);
            command.AddOption(outputOption);
            command.AddOption(compressOption);

            // Assign the handler using a lambda that accepts the options and the InvocationContext
            command.SetHandler((string input, string output, bool compress, InvocationContext context) =>
                HandleFlatten(input, output, compress, context),
                inputOption, outputOption, compressOption);

            return command;
        }

        private void HandleFlatten(string input, string output, bool compress, InvocationContext context)
        {
            var console = context.Console;
            var cancellationToken = context.GetCancellationToken();

            try
            {
                if (string.IsNullOrEmpty(output))
                {
                    output = Path.Combine(input, Path.GetFileName(input) + ".md");
                }

                var configManager = new ConfigurationManager("appsettings.json");
                var languageMap = configManager.GetLanguageMap();
                var ignoredPaths = configManager.GetIgnoredPaths();

                var fileHandlers = new List<IFileHandler>
                {
                    new CodeFileHandler(languageMap),
                    new ImageFileHandler(),
                    new PdfFileHandler(),
                    new SqliteFileHandler()
                };

                var fileHelper = new FileHelper(fileHandlers);

                input = Path.GetFullPath(input);
                output = Path.GetFullPath(output);

                Log.Information($"Input Path: {input}");
                Log.Information($"Output Path: {output}");
                Log.Information($"Compress: {compress}");

                input = Environment.ExpandEnvironmentVariables(input);
                output = Environment.ExpandEnvironmentVariables(output);

                if (!Directory.Exists(input))
                {
                    Log.Error($"The input directory {input} does not exist.");
                    context.ExitCode = 1;
                    return;
                }

                var files = Directory.EnumerateFiles(input, "*", SearchOption.AllDirectories)
                    .Where(file => !ignoredPaths.Any(ignored =>
                        file.Contains(Path.DirectorySeparatorChar + ignored + Path.DirectorySeparatorChar) ||
                        file.Contains(Path.DirectorySeparatorChar + ignored + Path.AltDirectorySeparatorChar) ||
                        file.EndsWith(Path.DirectorySeparatorChar + ignored) ||
                        file.EndsWith(Path.AltDirectorySeparatorChar + ignored)))
                    .ToList();

                Log.Information($"Found {files.Count} files to process.");

                var markdownContent = new MarkdownDocument();

                markdownContent.AddContent(new Header(1, Path.GetFileName(input)));

                foreach (var filePath in files)
                {
                    try
                    {
                        var fileMetadata = fileHelper.ProcessFile(filePath);
                        if (fileMetadata != null)
                        {
                            AppendFileContent(markdownContent, input, fileMetadata, compress);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, $"Failed to process file {filePath}. Skipping.");
                    }
                }

                File.WriteAllText(output, markdownContent.ToString());
                Log.Information($"Output written to {output}");
                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while flattening the codebase.");
                context.ExitCode = 1;
            }
            finally
            {
                Logger.CloseAndFlush();
            }
        }

        private void AppendFileContent(
            MarkdownDocument markdownContent,
            string rootFolder,
            FileMetadata fileMetadata,
            bool compress)
        {
            var relativePath = Path.GetRelativePath(rootFolder, fileMetadata.Path);
            markdownContent.AddContent(new Header(2, relativePath.Replace('\\', '/')));
            var content = compress ? CompressContent(fileMetadata.Content) : fileMetadata.Content;
            markdownContent.AddContent(new CodeBlock(fileMetadata.Language, content));
        }

        private string CompressContent(string content)
        {
            // Implement compression logic as needed
            return content.Replace(" ", string.Empty);
        }
    }
}
