using RepoScribe.Core.ContentItems;

namespace RepoScribe.Core.Abstractions
{
    public interface IContentExtractor
    {
        bool CanExtract(string input);
        ContentItem ExtractContent(string input);
    }
}
