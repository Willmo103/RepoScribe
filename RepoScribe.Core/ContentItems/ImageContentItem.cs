using RepoScribe.Core.Services;
using System.Threading.Tasks;

namespace RepoScribe.Core.ContentItems
{
    public class ImageContentItem : ContentItem
    {
        public string ImageMetadata { get; set; }
        public byte[] ImageData { get; set; }

        public override void Ingest()
        {
            // Implement image ingestion logic here
            // For example, extract metadata, generate thumbnails, etc.
        }

        public override async Task SaveAsync()
        {
            await LocalDatabaseService.Instance.SaveContentItemAsync(this);
            await ChromaDbService.Instance.UpsertAsync(this);
        }
    }
}
