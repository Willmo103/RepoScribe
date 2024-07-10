using System.Text;

namespace CodeFlattener
{
    public class CodeFlattener
    {
        public void FlattenCodebase(string rootFolder, string outputFile)
        {
            if (!Directory.Exists(rootFolder))
            {
                throw new DirectoryNotFoundException($"Directory not found: {rootFolder}");
            }

            StringBuilder markdownContent = new StringBuilder();
            foreach (string filePath in Directory.GetFiles(rootFolder, "*.*", SearchOption.AllDirectories))
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