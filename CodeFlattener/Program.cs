using System;
using System.IO;
using System.Linq;

namespace CodeFlattener
{
    public class Program
    {
        public static readonly string[] DefaultExtensions = {
            ".cs", ".js", ".ts", ".html", ".css", ".scss", ".json", ".xml", ".yml", ".yaml",
            ".md", ".txt", ".csproj", ".csv", ".sql", ".sh", ".bat", ".ps1", ".psm1", ".psd1",
            ".ps1xml", ".xaml", ".config", ".gitignore", ".editorconfig", ".dockerignore",
            ".dockerfile", ".sln"
        };

        public static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                ProcessCommandLineArgs(args);
            }
            else
            {
                ProcessInteractiveMode();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void ProcessCommandLineArgs(string[] args)
        {
            string rootFolder = args[0];
            string outputFile = args[1];
            string[] fileExtensions = args.Length > 2 ? args.Skip(2).ToArray() : DefaultExtensions;

            ValidateAndFlattenCodebase(rootFolder, outputFile, fileExtensions);
        }
        private static string ValidateDirectoryExists(string path)
        {
            return Directory.Exists(path) ? path : throw new DirectoryNotFoundException($"Directory not found: {path}");
        }

        private static void ProcessInteractiveMode()
        {
            string rootFolder = GetValidatedInput("Enter the root folder path:", ValidateDirectoryExists);
            string outputFile = GetValidatedInput($"Enter the output file name (including .md extension) Default: `{rootFolder}_Code.md`:",
                                                  input => string.IsNullOrWhiteSpace(input) ? $"{rootFolder}_Code.md" : input);

            string[] fileExtensions = PromptForFileExtensions();

            ValidateAndFlattenCodebase(rootFolder, outputFile, fileExtensions);
        }

        private static string[] PromptForFileExtensions()
        {
            string useExtensions = GetValidatedInput("Would you like to filter by file extensions? (y/n)",
                input => {
                    string loweredInput = input.ToLower();
                    return loweredInput is "y" or "n" or "" ? loweredInput : throw new ArgumentException("Please enter 'y', 'n', or press Enter");
                });

            return useExtensions == "y" ? GetUserSpecifiedExtensions() : DefaultExtensions;
        }

        private static T GetValidatedInput<T>(string prompt, Func<string, T> validator)
        {
            while (true)
            {
                Console.WriteLine($"{prompt}\n>");
                string input = Console.ReadLine() ?? string.Empty;
                try
                {
                    return validator(input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}");
                }
            }
        }

        private static string[] GetUserSpecifiedExtensions()
        {
            return GetValidatedInput("Enter the file extensions separated by commas (e.g. `cs,js,ts`):",
                input =>
                {
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("No extensions entered. Using default extensions.");
                        return DefaultExtensions;
                    }
                    else
                    {
                        return input.Split(',')
                            .Select(e => e.Trim().StartsWith(".") ? e.Trim() : "." + e.Trim())
                            .ToArray();
                    }
                });
        }
        private static void ValidateAndFlattenCodebase(string rootFolder, string outputFile, string[] fileExtensions)
        {
            try
            {
                ValidateDirectoryExists(rootFolder);
                CodeFlattener flattener = new CodeFlattener();
                flattener.FlattenCodebase(rootFolder, outputFile, fileExtensions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}