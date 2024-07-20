using Microsoft.Extensions.Configuration;

namespace CodeFlattener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: CodeFlattener <rootFolder> <outputFile>");
                return;
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string[] acceptedFileTypes = config.GetSection("AcceptedFileTypes").Value.Split(',');
            string[] ignoredPaths = config.GetSection("IgnoredPaths").Value.Split(',');

            string rootFolder = args[0];
            string outputFile = args[1];

            ValidateAndFlattenCodebase(rootFolder, outputFile, acceptedFileTypes, ignoredPaths);

            Console.WriteLine("Process completed. Press any key to exit...");
            Console.ReadKey();
        }

        private static void ValidateAndFlattenCodebase(string rootFolder, string outputFile, string[] acceptedFileTypes, string[] ignoredPaths)
        {
            try
            {
                string absoluteRootFolder = Path.GetFullPath(rootFolder);
                ValidateDirectoryExists(absoluteRootFolder);

                string absoluteOutputFile = Path.IsPathRooted(outputFile) ? outputFile : Path.Combine(Directory.GetCurrentDirectory(), outputFile);

                CodeFlattener flattener = new CodeFlattener();
                flattener.FlattenCodebase(absoluteRootFolder, absoluteOutputFile, acceptedFileTypes, ignoredPaths);
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