using CodeFlattener.Core.DataModels;
using Microsoft.Data.Sqlite; // Add this using directive

namespace CodeFlattener.Core.FileHandlers
{
    public class SqliteFileHandler : IFileHandler
    {
        private readonly List<string> _supportedExtensions = new List<string> { ".sqlite", ".db", ".sqlite3" };

        public bool CanHandle(string extension)
        {
            return _supportedExtensions.Contains(extension.ToLower());
        }

        public FileMetadata ProcessFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var tables = GetTables(filePath);
            string content = $"Tables: {string.Join(", ", tables)}";

            var lines = content.Split('\n')
                .Select((line, index) => new LineContent { Number = index + 1, Content = line.Trim() })
                .ToList();

            return new FileMetadata
            {
                Path = filePath,
                Owner = fileInfo.GetAccessControl()
                    .GetOwner(typeof(System.Security.Principal.NTAccount)).ToString() ?? "",
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
