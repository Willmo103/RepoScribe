using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CodeFlattener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2 || args.Length > 3)
                {
                    Console.WriteLine("Usage: CodeFlattener <rootFolder> <outputFile> [-c|-Compress]");
                    return;
                }

                string rootFolder = args[0];
                string outputFile = args[1];
                bool compress = args.Length == 3 && (args[2] == "-c" || args[2] == "-Compress");

                string configPath = GetConfigPath();
                if (string.IsNullOrEmpty(configPath))
                {
                    Console.WriteLine("Error: Unable to locate appsettings.json");
                    return;
                }

                var config = new ConfigurationBuilder()
                    .AddJsonFile(configPath, optional: false, reloadOnChange: true)
                    .Build();

                string[] acceptedFileTypes = config.GetSection("AcceptedFileTypes").Value?.Split(',') ?? Array.Empty<string>();
                string[] ignoredPaths = config.GetSection("IgnoredPaths").Value?.Split(',') ?? Array.Empty<string>();

                if (acceptedFileTypes.Length == 0 || ignoredPaths.Length == 0)
                {
                    Console.WriteLine("Error: Configuration sections are missing, empty, or improperly formatted.");
                    return;
                }

                ValidateAndFlattenCodebase(rootFolder, outputFile, acceptedFileTypes, ignoredPaths, compress);

                Console.WriteLine("Process completed. Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unhandled error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static string GetConfigPath()
        {
            string[] possiblePaths = new string[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"),
                Path.Combine(Environment.CurrentDirectory, "appsettings.json"),
                "appsettings.json"
            };

            return possiblePaths.FirstOrDefault(File.Exists);
        }

        private static void ValidateAndFlattenCodebase(string rootFolder, string outputFile, string[] acceptedFileTypes, string[] ignoredPaths, bool compress)
        {
            try
            {
                string absoluteRootFolder = Path.GetFullPath(rootFolder);
                ValidateDirectoryExists(absoluteRootFolder);

                string absoluteOutputFile = Path.IsPathRooted(outputFile) ? outputFile : Path.Combine(Directory.GetCurrentDirectory(), outputFile);

                CodeFlattener flattener = new CodeFlattener();
                flattener.FlattenCodebase(absoluteRootFolder, absoluteOutputFile, acceptedFileTypes, ignoredPaths, compress);

                Console.WriteLine($"Output written to: {absoluteOutputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void ValidateDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Directory not found: {path}");
            }
        }
    }
}