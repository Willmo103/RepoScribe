namespace CodeFlattener
{
    public static class FileHelper
    {
        public static string GetLanguageIdentifier(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".cs" => "csharp",
                ".js" => "javascript",
                ".ts" => "typescript",
                ".py" => "python",
                ".html" => "html",
                ".css" => "css",
                ".scss" => "scss",
                ".json" => "json",
                ".xml" => "xml",
                ".yml" or ".yaml" => "yaml",
                ".md" => "markdown",
                ".txt" => "plaintext",
                ".sql" => "sql",
                ".sh" => "shell",
                ".bat" => "batch",
                ".ps1" or ".psm1" or ".psd1" => "powershell",
                ".xaml" => "xaml",
                ".config" => "xml",
                ".dockerfile" => "dockerfile",
                ".gitignore" => "plaintext",
                ".editorconfig" => "plaintext",
                _ => ""
            };
        }
    }
}