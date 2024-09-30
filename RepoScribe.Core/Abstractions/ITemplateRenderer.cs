using RepoScribe.Core.ContentItems;

namespace RepoScribe.Core.Abstractions
{
    public interface ITemplateRenderer : IRenderer
    {
        string Render(ContentItem contentItem, string template);
    }
}
