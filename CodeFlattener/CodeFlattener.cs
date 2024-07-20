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
            markdownContent.AppendLine($"# Codebase Parsed From: {Path.GetFullPath(rootFolder)}");

            var files = GetFilteredFiles(rootFolder, acceptedFileTypes, ignoredPaths);

            foreach (string filePath in files)
            {
                AppendFileContent(markdownContent, rootFolder, filePath);
            }

            File.WriteAllText(outputFile, markdownContent.ToString());
            Console.WriteLine($"COMPLETE!\nYour codebase has been flattened to: {outputFile}\n\n");
        }

        private IEnumerable<string> GetFilteredFiles(string rootFolder, string[]? fileExtensions, string[]? ignoredPaths)
        {
            var files = Directory.EnumerateFiles(rootFolder, "*.*", SearchOption.AllDirectories);

            if (fileExtensions != null && fileExtensions.Length > 0)
            {
                files = files.Where(f => fileExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase));
            }

            if (ignoredPaths != null && ignoredPaths.Length > 0)
            {
                files = files.Where(f => !ignoredPaths.Any(ip =>
                    f.Contains(Path.DirectorySeparatorChar + ip + Path.DirectorySeparatorChar) ||
                    f.EndsWith(Path.DirectorySeparatorChar + ip) ||
                    Path.GetFileName(f).Equals(ip, StringComparison.OrdinalIgnoreCase)));
            }

            return files;
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