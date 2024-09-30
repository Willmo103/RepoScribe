namespace RepoScribe.Core.DataModels
{
    public class CacheEntry
    {
        public Guid CacheEntryId { get; private set; }
        public List<ClusterIndex> ClusterIndices { get; set; }
        private Guid KeyWordEmbeddingId { get; }
        public string[] KeyWords { get; set; }
    }
}
