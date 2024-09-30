using RepoScribe.Core.DataModels;
using RepoScribe.Core.Services;
using System.Threading.Tasks;

namespace RepoScribe.Core.ContentItems
{
    public class CodeContentItem : ContentItem
    {
        public List<LineContent> Lines { get; set; } = new List<LineContent>();

        public override void Ingest()
        {
            // Implement code ingestion logic here
            // For example, parse code, analyze dependencies, etc.
        }

        public override async Task SaveAsync()
        {
            // Save to local database
            await LocalDatabaseService.Instance.SaveContentItemAsync(this);

            // Save to ChromaDB
            await ChromaDbService.Instance.UpsertAsync(this);
        }
    }
}
