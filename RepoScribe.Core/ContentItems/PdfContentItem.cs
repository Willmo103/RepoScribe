using RepoScribe.Core.DataModels;

namespace RepoScribe.Core.ContentItems
{
    public class PdfContentItem : ContentItem
    {
        public List<LineContent> Lines { get; set; } = new List<LineContent>();
    }
}
