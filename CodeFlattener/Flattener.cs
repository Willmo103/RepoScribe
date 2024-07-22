using System.Text;
using System.Text.RegularExpressions;

namespace CodeFlattener
{
    public class Flattener
    {
        public void FlattenCodebase(string rootFolder, string outputFile, string[] acceptedFileTypes, string[] ignoredPaths, bool compress)
        {
            if (!Directory.Exists(rootFolder))
            {
                throw new DirectoryNotFoundException($"Directory not found: {rootFolder}");
            }

            StringBuilder markdownContent = new StringBuilder();

            var files = GetFilteredFiles(rootFolder, acceptedFileTypes, ignoredPaths);

            foreach (string filePath in files)
            {
                AppendFileContent(markdownContent, rootFolder, filePath, compress);
            }

            File.WriteAllText(outputFile, markdownContent.ToString());
        }

        private IEnumerable<string> GetFilteredFiles(string rootFolder, string[] acceptedFileTypes, string[] ignoredPaths)
        {
            return Directory.EnumerateFiles(rootFolder, "*.*", SearchOption.AllDirectories)
                .Where(file =>
                {
                    string relativePath = Path.GetRelativePath(rootFolder, file);
                    return !IsPathIgnored(relativePath, ignoredPaths);
                })
                .Where(file => acceptedFileTypes.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase));
        }

        private bool IsPathIgnored(string path, string[] ignoredPaths)
        {
            return ignoredPaths.Any(ignoredPath =>
                path.StartsWith(ignoredPath, StringComparison.OrdinalIgnoreCase) ||
                path.Split(Path.DirectorySeparatorChar).Any(segment =>
                    segment.Equals(ignoredPath, StringComparison.OrdinalIgnoreCase)));
        }

        private void AppendFileContent(StringBuilder markdownContent, string rootFolder, string filePath, bool compress)
        {
            string relativePath = Path.GetRelativePath(rootFolder, filePath);
            markdownContent.AppendLine($"# {relativePath.Replace('\\', '/')}");

            string languageIdentifier = FileHelper.GetLanguageIdentifier(filePath);
            markdownContent.AppendLine($"```{languageIdentifier}");

            string content = File.ReadAllText(filePath);
            if (compress)
            {
                content = CompressContent(content);
            }

            markdownContent.AppendLine(content);
            markdownContent.AppendLine("```");
            markdownContent.AppendLine();
        }

        private string CompressContent(string content)
        {
            // Remove all whitespace except for single spaces between words
            content = Regex.Replace(content, @"\s+", " ");
            // Remove spaces after certain characters
            content = Regex.Replace(content, @"(\(|\[|{) ", "$1");
            // Remove spaces before certain characters
            content = Regex.Replace(content, @" (\)|\]|}|,|;)", "$1");
            return content.Trim();
        }
    }
}