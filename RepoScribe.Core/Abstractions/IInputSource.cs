using RepoScribe.Core.ContentItems;

namespace RepoScribe.Core.Abstractions
{
    public interface IInputSource
    {
        ContentItem GetContentItem();
    }
}
