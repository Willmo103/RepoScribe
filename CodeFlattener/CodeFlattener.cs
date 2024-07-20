using System.Text;

namespace CodeFlattener
{
    public class CodeFlattener
    {
        public void FlattenCodebase(string rootFolder, string outputFile, string[] acceptedFileTypes, string[] ignoredPaths)
        {
            if (!Directory.Exists(rootFolder))
            {
                throw new DirectoryNotFoundException($"Directory not found: {rootFolder}");
            }

            StringBuilder markdownContent = new StringBuilder();

            var files = GetFilteredFiles(rootFolder, acceptedFileTypes, ignoredPaths);

            foreach (string filePath in files)
            {
                AppendFileContent(markdownContent, rootFolder, filePath);
            }

            File.WriteAllText(outputFile, markdownContent.ToString());
        }

        private IEnumerable<string> GetFilteredFiles(string rootFolder, string[] acceptedFileTypes, string[] ignoredPaths)
        {
            return Directory.EnumerateFiles(rootFolder, "*.*", SearchOption.AllDirectories)
                .Where(file => !ignoredPaths.Any(path => file.Contains(path, StringComparison.OrdinalIgnoreCase)))
                .Where(file => acceptedFileTypes.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase));
        }

        private void AppendFileContent(StringBuilder markdownContent, string rootFolder, string filePath)
        {
            string relativePath = Path.GetRelativePath(rootFolder, filePath);
            markdownContent.AppendLine($"# {relativePath.Replace('\\', '/')}");

            string languageIdentifier = FileHelper.GetLanguageIdentifier(filePath);
            markdownContent.AppendLine($"```{languageIdentifier}");
            markdownContent.AppendLine(File.ReadAllText(filePath));
            markdownContent.AppendLine("```");
            markdownContent.AppendLine();
        }
    }
}