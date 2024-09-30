using RepoScribe.Core.DataModels.Markdown;
using RepoScribe.Core.Abstractions;

namespace RepoScribe.Core.ContentItems
{
    public abstract class ContentItem
    {
        public int Id { get; set; } // Primary Key for ORM
        public string Path { get; set; }
        public string Owner { get; set; }
        public DateTime LastModified { get; set; }
        public double SizeMB { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
        public Domain Domain { get; set; }
        public ContextualInputSource ContextSource { get; set; }

        // Navigation properties or additional relationships can be added here
    }
}
