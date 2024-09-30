namespace RepoScribe.Core.ContentItems
{
    public class RepositoryContentItem : ContentItem
    {
        public string Url { get; set; }
        public string Author { get; set; }
        public string Readme { get; set; }
        public List<ContentItem> Files { get; set; } = new List<ContentItem>();
    }
}
