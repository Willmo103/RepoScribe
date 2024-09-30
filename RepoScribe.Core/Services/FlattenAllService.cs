using System.Diagnostics;
using RepoScribe.Core.Abstractions;
using RepoScribe.Core.Helpers;
using RepoScribe.Core.Utilities;
using Serilog;

namespace RepoScribe.Core.Services
{
    public class FlattenAllService
    {
        private readonly string _codeFlattenerPath;
        private readonly string _outputDirectory;
        private readonly InputProcessor _inputProcessor;
        private readonly IRenderer _renderer;

        public FlattenAllService(string codeFlattenerPath, string outputDirectory, InputProcessor inputProcessor, IRenderer renderer)
        {
            _codeFlattenerPath = codeFlattenerPath;
            _outputDirectory = outputDirectory;
            _inputProcessor = inputProcessor;
            _renderer = renderer;
        }

        public async Task FlattenAllAsync()
        {
            if (!File.Exists(_codeFlattenerPath))
            {
                Log.Error($"CodeFlattener.exe not found at {_codeFlattenerPath}");
                return;
            }

            Log.Information("Starting Flatten-All process...");

            var directories = Directory.GetDirectories(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories)
                                      .Where(dir => Directory.Exists(Path.Combine(dir, ".git")))
                                      .ToList();

            int total = directories.Count;
            int processed = 0;
            int success = 0;
            int failed = 0;

            foreach (var dir in directories)
            {
                processed++;
                var baseName = new DirectoryInfo(dir).Name;
                var dottedPath = PathUtility.ConvertFilePathToDottedPath(dir);
                var outputFile = Path.Combine(_outputDirectory, $"{dottedPath}.md");

                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = _codeFlattenerPath,
                            Arguments = $"\"{dir}\" \"{outputFile}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };

                    process.Start();
                    string stdout = await process.StandardOutput.ReadToEndAsync();
                    string stderr = await process.StandardError.ReadToEndAsync();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        Log.Information($"Successfully processed {baseName}. Output saved to {outputFile}");
                        success++;
                    }
                    else
                    {
                        Log.Error($"Error processing {baseName}. Exit code: {process.ExitCode}");
                        Log.Error($"Error details: {stderr}");
                        failed++;
                    }

                    double percentComplete = Math.Round((double)processed / total * 100, 2);
                    Log.Information($"Progress: {percentComplete}% ({processed}/{total})");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Failed to process directory {dir}");
                    failed++;
                }
            }

            Log.Information("Flatten-All process complete.");
            Log.Information($"Total directories processed: {processed}");
            Log.Information($"Successful: {success}");
            Log.Information($"Failed: {failed}");
        }
    }
}
