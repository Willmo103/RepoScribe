using RepoScribe.Core.DataModels;
using RepoScribe.Core.Services;
using System.Threading.Tasks;

namespace RepoScribe.Core.ContentItems
{
    public class PdfContentItem : ContentItem
    {
        public List<LineContent> Lines { get; set; } = new List<LineContent>();

        public override void Ingest()
        {
            // Implement PDF ingestion logic here
            // For example, extract text, index pages, etc.
        }

        public override async Task SaveAsync()
        {
            await LocalDatabaseService.Instance.SaveContentItemAsync(this);
            await ChromaDbService.Instance.UpsertAsync(this);
        }
    }
}
