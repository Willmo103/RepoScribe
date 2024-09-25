namespace RepoScribe.Core.DataModels
{
    public class RepositoryMetadata
    {
        public string Url { get; set; }
        public string Author { get; set; }
        public string Readme { get; set; }
        public List<FileMetadata> Files { get; set; } = new List<FileMetadata>();
    }
}
