using RepoScribe.Core.Abstractions;
using RepoScribe.Core.Utilities;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace RepoScribe.Core.Services
{
    public class MarkdownExtractor
    {
        private readonly string _inputDirectory;
        private readonly IRenderer _renderer;

        public MarkdownExtractor(string inputDirectory, IRenderer renderer)
        {
            _inputDirectory = inputDirectory;
            _renderer = renderer;
        }

        public IEnumerable<string> ExtractCodeBlocks()
        {
            var markdownFiles = Directory.GetFiles(_inputDirectory, "*.md", SearchOption.AllDirectories);

            foreach (var file in markdownFiles)
            {
                var content = File.ReadAllText(file);
                var matches = Regex.Matches(content, @"(?ms)^# (.+?)\r?\n```(\w+)\r?\n(.+?)\r?\n```");

                foreach (Match match in matches)
                {
                    var path = PathUtility.ConvertDottedPathToFilePath(match.Groups[1].Value.Trim());
                    var codeContent = match.Groups[3].Value.Trim();
                    var id = HashUtility.GetUniqueId(codeContent);

                    var codeObject = new
                    {
                        Id = id,
                        Path = path,
                        Content = codeContent
                    };

                    yield return JsonConvert.SerializeObject(codeObject);
                }
            }
        }
    }
}
