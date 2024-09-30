namespace RepoScribe.Core.ContentItems
{
    public class ImageContentItem : ContentItem
    {
        public string ImageMetadata { get; set; }
        
        // Store image data as byte array
        public byte[] ImageData { get; set; } 
    }
}
