using RepoScribe.Core.Abstractions;
using RepoScribe.Core.DataModels;
using RepoScribe.Core.DataModels.Enums;

namespace RepoScribe.Core.ContentItems
{
    public abstract class ContentItem : Metadata
    {
        public int Id { get; set; }
        public Domain Domain { get; set; }
        public ContextualInputSource ContextSource { get; set; }
    }
}
