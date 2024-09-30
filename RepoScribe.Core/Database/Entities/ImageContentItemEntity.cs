namespace RepoScribe.Core.Database.Entities
{
    public class ImageContentItemEntity : ContentItemEntity
    {
        public string ImageMetadata { get; set; }
        public byte[] ImageData { get; set; }
    }
}
