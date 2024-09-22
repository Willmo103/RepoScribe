using CodeFlattener.Core.DataModels;
using CodeFlattener.Core.FileHandlers;
using CodeFlattener.Core.Helpers;
using CodeFlattener.Core.Utilities;
using Serilog;
using System;
using System.CommandLine;
using System.Text;

namespace FlattenCodebase
{
    class Program
    {
        static int Main(string[] args)
        {
            Logger.Initialize();

            var rootCommand = new RootCommand("FlattenCodebase Tool");

            var inputOption = new Option<string>(
                new[] { "--input", "-i" },
                "The root directory to process");

            var outputOption = new Option<string>(
                new[] { "--output", "-o" },
            "The output Markdown file");

            var compressOption = new Option<bool>(
                new[] { "--compress", "-c" },
                () => false,
                "Enable content compression");

            rootCommand.AddOption(inputOption);
            rootCommand.AddOption(outputOption);
            rootCommand.AddOption(compressOption);

            rootCommand.SetHandler((input, output, compress) =>
            {
                try
                {
                    var configManager = new ConfigurationManager("appsettings.json");
                    var languageMap = configManager.GetLanguageMap();
                    var ignoredPaths = configManager.GetIgnoredPaths();

                    var fileHandlers = new List<IFileHandler>
                    {
                        new CodeFileHandler(languageMap)
                    };

                    var fileHelper = new FileHelper(fileHandlers);

                    var files = Directory.EnumerateFiles(input, "*.*", SearchOption.AllDirectories)
                        .Where(file => !ignoredPaths.Any(ignored => file.Contains(ignored)))
                        .ToList();

                    var markdownContent = new StringBuilder();

                    foreach (var filePath in files)
                    {
                        var fileMetadata = fileHelper.ProcessFile(filePath);
                        if (fileMetadata != null)
                        {
                            AppendFileContent(markdownContent, input, fileMetadata, compress);
                        }
                    }

                    if (!string.IsNullOrEmpty(output))
                    {
                        File.WriteAllText(output, markdownContent.ToString());
                        Log.Information($"Output written to {output}");
                    }
                    else
                    {
                        Console.WriteLine(markdownContent.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred while flattening the codebase.");
                }
                finally
                {
                    Logger.CloseAndFlush();
                }
            }, inputOption, outputOption, compressOption);

            return rootCommand.InvokeAsync(args).Result;
        }

        private static void AppendFileContent(
            StringBuilder markdownContent,
            string rootFolder,
            FileMetadata fileMetadata,
            bool compress)
        {
            var relativePath = Path.GetRelativePath(rootFolder, fileMetadata.Path);
            markdownContent.AppendLine($"# {relativePath.Replace('\\', '/')}");

            markdownContent.AppendLine($"```{fileMetadata.Language}");

            var content = compress ? CompressContent(fileMetadata.Content) : fileMetadata.Content;

            markdownContent.AppendLine(content);
            markdownContent.AppendLine("```");
            markdownContent.AppendLine();
        }

        private static string CompressContent(string content)
        {
            // Implement compression logic as needed
            return content.Replace(" ", string.Empty);
        }
    }
}
