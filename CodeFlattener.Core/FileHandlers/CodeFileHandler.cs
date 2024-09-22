using CodeFlattener.Core.DataModels;

namespace CodeFlattener.Core.FileHandlers
{
    public class CodeFileHandler : IFileHandler
    {
        private readonly Dictionary<string, string> _languageMap;

        public CodeFileHandler(Dictionary<string, string> languageMap)
        {
            _languageMap = languageMap;
        }

        public bool CanHandle(string extension)
        {
            return _languageMap.ContainsKey(extension);
        }

        public FileMetadata ProcessFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var ext = fileInfo.Extension.ToLower() ?? fileInfo.Name;
            var fileSecurity = fileInfo.GetAccessControl();
            var owner = fileSecurity.GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

            var lines = File.ReadLines(filePath)
                .Select((line, index) => new LineContent { Number = index + 1, Content = line })
                .ToList();

            return new FileMetadata
            {
                Path = filePath,
                Owner = owner,
                LastModified = fileInfo.LastWriteTime,
                SizeMB = fileInfo.Length / (1024.0 * 1024.0),
                Language = _languageMap.GetValueOrDefault(ext, ""),
                Content = File.ReadAllText(filePath),
                Lines = lines
            };
        }
    }
}
