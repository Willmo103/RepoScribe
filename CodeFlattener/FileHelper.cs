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
                ".py" => "python",
                ".html" => "html",
                ".css" => "css",
                ".json" => "json",
                _ => ""
            };
        }
    }
}