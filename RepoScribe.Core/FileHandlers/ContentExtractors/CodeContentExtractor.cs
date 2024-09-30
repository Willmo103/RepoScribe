using RepoScribe.Core.Abstractions;
using RepoScribe.Core.ContentItems;
using RepoScribe.Core.DataModels;
using RepoScribe.Core.Utilities;

namespace RepoScribe.Core.FileHandlers.ContentExtractors
{
    public class CodeContentExtractor : IContentExtractor
    {
        private readonly Dictionary<string, string> _languageMap;

        public CodeContentExtractor(Dictionary<string, string> languageMap)
        {
            _languageMap = languageMap;
        }

        public bool CanExtract(string input)
        {
            var extension = Path.GetExtension(input).ToLower();
            return _languageMap.ContainsKey(extension);
        }

        public ContentItem ExtractContent(string input)
        {
            var fileInfo = new FileInfo(input);
            var ext = fileInfo.Extension.ToLower();
            var fileSecurity = fileInfo.GetAccessControl();
            var owner = fileSecurity.GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

            var lines = File.ReadLines(input)
                .Select((line, index) => new LineContent { Number = index + 1, Content = line })
                .ToList();

            return new CodeContentItem
            {
                Path = input,
                Owner = owner,
                LastModified = fileInfo.LastWriteTime,
                SizeMB = fileInfo.Length / (1024.0 * 1024.0),
                Language = _languageMap.GetValueOrDefault(ext, ""),
                Content = File.ReadAllText(input),
                Lines = lines
            };
        }
    }
}
