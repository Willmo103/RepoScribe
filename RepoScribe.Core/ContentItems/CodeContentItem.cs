using RepoScribe.Core.DataModels;

namespace RepoScribe.Core.ContentItems
{
    public class CodeContentItem : ContentItem
    {
        public List<LineContent> Lines { get; set; } = new List<LineContent>();
        //public FileMetadata Metadata { get; set; } // Create a Combined Metadata class
        //public RepositoryMetadata Repository { get; set; }
        //public Module Module { get; set; }
        //public List<CodeContentItem> Dependencies { get; set; } = new List<CodeContentItem>();
        //public List<CodeContentItem> Dependents { get; set; } = new List<CodeContentItem>();
        //public Dictionary<string, string> 
    }
}
