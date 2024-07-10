using System.IO;

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
        }

        private static void ProcessCommandLineArgs(string[] args)
        {
            string rootFolder = args[0];
            string outputFile = args[1];
            string[] fileExtensions = args.Length > 2 ? args.Skip(2).ToArray() : DefaultExtensions;

            if (!Path.Exists(rootFolder))
            {
                throw new DirectoryNotFoundException("Directory not found: invalid_path");
            }

            FlattenCodebase(rootFolder, outputFile, fileExtensions);
        }

        private static void ProcessInteractiveMode()
        {
            Console.WriteLine("Code Flattener\n");
            Console.Write("Enter the root folder path: ");
            string rootFolder = Console.ReadLine() ?? string.Empty;

            while (string.IsNullOrWhiteSpace(rootFolder) || !Directory.Exists(rootFolder))
            {
                Console.WriteLine($"Invalid folder path! Folder: {rootFolder} does not exist");
                Console.Write("Enter the root folder path: ");
                rootFolder = Console.ReadLine() ?? string.Empty;
            }

            Console.Write("Enter the output file name (including .md extension) Default: `{file path}_Code.md`: ");
            string outputFile = Console.ReadLine() ?? $"{rootFolder}_Code.md";

            var useExtensions = string.Empty;
            Console.WriteLine("Would you like to filter by file extensions? (y/n)");
            while (useExtensions != "y" && useExtensions != "n")
            {
                Console.WriteLine("Invalid input");
                Console.WriteLine("Would you like to filter by file extensions? (y/n)");
                useExtensions = Console.ReadLine()?.ToLower() ?? string.Empty;
            }
            string[] fileExtensions = useExtensions == "y" ? GetUserSpecifiedExtensions() : DefaultExtensions;

            FlattenCodebase(rootFolder, outputFile, fileExtensions);
        }

        private static string[] GetUserSpecifiedExtensions()
        {
            Console.Write("Enter the file extensions separated by commas (e.g. `cs,js,ts`): ");
            string extensionsInput = Console.ReadLine() ?? string.Empty;
            return extensionsInput.Split(',').Select(e => e.Trim().StartsWith(".") ? e.Trim() : "." + e.Trim()).ToArray();
        }

        private static void FlattenCodebase(string rootFolder, string outputFile, string[] fileExtensions)
        {
            try
            {
                CodeFlattener flattener = new CodeFlattener();
                flattener.FlattenCodebase(rootFolder, outputFile, fileExtensions);
                Console.WriteLine($"Done! Output written to {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}