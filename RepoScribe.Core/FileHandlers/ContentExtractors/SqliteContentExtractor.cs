using RepoScribe.Core.Abstractions;
using RepoScribe.Core.ContentItems;
using Microsoft.Data.Sqlite;
using RepoScribe.Core.DataModels;

namespace RepoScribe.Core.FileHandlers.ContentExtractors
{
    public class SqliteContentExtractor : IContentExtractor
    {
        private readonly List<string> _supportedExtensions = new List<string> { ".sqlite", ".db", ".sqlite3" };

        public bool CanExtract(string input)
        {
            var extension = Path.GetExtension(input).ToLower();
            return _supportedExtensions.Contains(extension);
        }

        public ContentItem ExtractContent(string input)
        {
            var fileInfo = new FileInfo(input);
            var owner = fileInfo.GetAccessControl()
                .GetOwner(typeof(System.Security.Principal.NTAccount)).ToString() ?? "";

            var tables = GetTables(input);
            string content = $"Tables: {string.Join(", ", tables)}";

            var lines = content.Split('\n')
                .Select((line, index) => new LineContent { Number = index + 1, Content = line.Trim() })
                .ToList();

            return new PdfContentItem // Assuming PdfContentItem is reused; consider creating a separate entity if needed
            {
                Path = input,
                Owner = owner,
                LastModified = fileInfo.LastWriteTime,
                SizeMB = fileInfo.Length / (1024.0 * 1024.0),
                Language = "sqlite",
                Content = content,
                Lines = lines
            };
        }

        private List<string> GetTables(string filePath)
        {
            var tables = new List<string>();
            try
            {
                using (var connection = new SqliteConnection($"Data Source={filePath};Version=3;"))
                {
                    connection.Open();
                    using (var cmd = new SqliteCommand("SELECT name FROM sqlite_master WHERE type='table';", connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tables.Add($"Error extracting tables: {ex.Message}");
            }
            return tables;
        }
    }
}
