namespace CodeFlattener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // If args passed, use the first arg as the root folder path, and the second arg as the output file path else prompt the user for input
            if (args.Length == 2)
            {
                Console.WriteLine("Code Flattener\n");

                string rootFolder = args[0];
                string outputFile = args[1];
                
                if (!Directory.Exists(rootFolder))
                {
                    throw new DirectoryNotFoundException($"Directory not found: {rootFolder}");
                }
                
                try
                {
                    CodeFlattener flattener = new CodeFlattener();
                    flattener.FlattenCodebase(rootFolder, outputFile);
                    Console.WriteLine($"Done! Output written to {outputFile}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                return;
            }
            else
            {

                Console.WriteLine("Code Flattener\n");
                Console.Write("Enter the root folder path: ");
                string rootFolder = Console.ReadLine() ?? string.Empty;

                while (string.IsNullOrWhiteSpace(rootFolder) || !Directory.Exists(rootFolder))
                {
                    Console.WriteLine("Invalid folder path");
                    Console.Write("Enter the root folder path: ");
                    rootFolder = Console.ReadLine() ?? string.Empty;
                }

                Console.Write("Enter the output file name (including .md extension) Default: `{file path}_Code.md`: ");
                string outputFile = Console.ReadLine() ?? $"{rootFolder}_Code.md";

                try
                {
                    CodeFlattener flattener = new CodeFlattener();
                    flattener.FlattenCodebase(rootFolder, outputFile);
                    Console.WriteLine($"Done! Output written to {outputFile}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}