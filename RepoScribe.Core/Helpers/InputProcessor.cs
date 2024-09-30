using RepoScribe.Core.Abstractions;
using RepoScribe.Core.ContentItems;

namespace RepoScribe.Core.Helpers
{
    public class InputProcessor
    {
        private readonly List<IContentExtractor> _extractors;

        public InputProcessor(List<IContentExtractor> extractors)
        {
            _extractors = extractors;
        }

        public ContentItem ProcessInput(string input)
        {
            var extension = Path.GetExtension(input).ToLower();
            if (string.IsNullOrEmpty(extension))
            {
                extension = Path.GetFileName(input).ToLower();
            }

            var extractor = _extractors.FirstOrDefault(e => e.CanExtract(input));

            if (extractor != null)
            {
                return extractor.ExtractContent(input);
            }

            return null;
        }
    }
}
