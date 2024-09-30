using RepoScribe.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoScribe.Core.ContentItems
{
    public class RepositoryContentItem : ContentItem
    {
        public string Url { get; set; }
        public string Author { get; set; }
        public string Readme { get; set; }
        public List<ContentItem> Files { get; set; } = new List<ContentItem>();

        public override void Ingest()
        {
            // Implement repository ingestion logic here
            // For example, clone repo, parse files, etc.
        }

        public override async Task SaveAsync()
        {
            await LocalDatabaseService.Instance.SaveContentItemAsync(this);
            await ChromaDbService.Instance.UpsertAsync(this);

            // Optionally save nested files
            foreach (var file in Files)
            {
                file.Ingest();
                await file.SaveAsync();
            }
        }
    }
}
