using System.Text;

namespace CodeFlattener
{
    public class CodeFlattener
    {
        //Overload Method
        public void FlattenCodebase(string rootFolder, string outputFile)
        {
            FlattenCodebase(rootFolder, outputFile, null);
        }

        public void FlattenCodebase(string rootFolder, string outputFile, string[]? fileExtensions = null)
        {
            if (!Directory.Exists(rootFolder))
            {
                throw new DirectoryNotFoundException($"Directory not found: {rootFolder}");
            }

            StringBuilder markdownContent = new StringBuilder();

            var files = Directory.GetFiles(rootFolder, "*.*", SearchOption.AllDirectories);

            if (fileExtensions != null && fileExtensions.Length > 0)
            {
                files = files.Where(f => fileExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase)).ToArray();
            }

            foreach (string filePath in files)
            {
                string relativePath = Path.GetRelativePath(rootFolder, filePath);
                markdownContent.AppendLine($"# {relativePath.Replace('\\', '/')}");

                string languageIdentifier = FileHelper.GetLanguageIdentifier(filePath);
                markdownContent.AppendLine($"```{languageIdentifier}");
                markdownContent.AppendLine(File.ReadAllText(filePath));
                markdownContent.AppendLine("```");
                markdownContent.AppendLine();
            }

            File.WriteAllText(outputFile, markdownContent.ToString());
        }
    }
}