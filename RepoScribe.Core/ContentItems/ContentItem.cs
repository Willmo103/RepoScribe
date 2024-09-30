using RepoScribe.Core.Abstractions;
using RepoScribe.Core.DataModels.Enums;

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
        public Domain Domain { get; set; }  // Domain is now implemented
        public ContextualInputSource ContextSource { get; set; } // Contextual Input Source

        // Navigation properties or additional relationships can be added here
    }
}
