namespace RepoScribe.Core.DataModels
{
    public class FileMetadata
    {
        public string Path { get; set; }
        public string Owner { get; set; }
        public DateTime LastModified { get; set; }
        public double SizeMB { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
        public List<LineContent> Lines { get; set; }
    }
}
