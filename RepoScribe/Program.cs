using RepoScribe.Core.DataModels;
using RepoScribe.Core.FileHandlers;
using RepoScribe.Core.Helpers;
using RepoScribe.Core.Utilities;
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
            "The output Markdown file. Default: ${output.basename}.md");

            var compressOption = new Option<bool>(
                new[] { "--compress", "-c" },
                () => false,
                "Enable content compression");

            rootCommand.AddOption(inputOption);
            rootCommand.AddOption(outputOption);
            rootCommand.AddOption(compressOption);

            rootCommand.SetHandler((string input, string? output, bool compress) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(output))
                    {
                        // If no output file is specified, use the input directory name
                        output = Path.Combine(input, Path.GetFileName(input) + ".md");
                    }
                    var configManager = new ConfigurationManager("appsettings.json");
                    var languageMap = configManager.GetLanguageMap();
                    var ignoredPaths = configManager.GetIgnoredPaths();

                    var fileHandlers = new List<IFileHandler>
                    {
                        new CodeFileHandler(languageMap)
                    };

                    var fileHelper = new FileHelper(fileHandlers);

                    // Resolve any relative paths into absolute paths (if needed)
                    input = Path.GetFullPath(input);
                    output = Path.GetFullPath(output);
                    Log.Information($"Input Full Path: {input}");
                    Log.Information($"Output Full Path: {output}");
                    Log.Information($"Compress: {compress}");

                    // Resolve any environment variables in the input path
                    input = Environment.ExpandEnvironmentVariables(input);
                    output = Environment.ExpandEnvironmentVariables(output);

                    if (!Directory.Exists(input))
                    {
                        Log.Error($"The input directory {input} does not exist.");
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

                    var markdownContent = new StringBuilder();

                    // Set the title of the markdown file to the root folder name
                    markdownContent.AppendLine($"# {Path.GetFileName(input)}");

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
