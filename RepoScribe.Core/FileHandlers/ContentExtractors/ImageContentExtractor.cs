using RepoScribe.Core.Abstractions;
using RepoScribe.Core.ContentItems;
using SixLabors.ImageSharp;

namespace RepoScribe.Core.FileHandlers.ContentExtractors
{
    public class ImageContentExtractor : IContentExtractor
    {
        private readonly List<string> _supportedExtensions = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

        public bool CanExtract(string input)
        {
            var extension = Path.GetExtension(input).ToLower();
            return _supportedExtensions.Contains(extension);
        }

        public ContentItem ExtractContent(string input)
        {
            var fileInfo = new FileInfo(input);
            var owner = fileInfo.GetAccessControl()
                .GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

            using var image = Image.Load(input);
            var metadata = image.Metadata.ToString(); // Simplified for example

            return new ImageContentItem
            {
                Path = input,
                Owner = owner,
                LastModified = fileInfo.LastWriteTime,
                SizeMB = fileInfo.Length / (1024.0 * 1024.0),
                Language = "image",
                Content = null, // Binary content is not included
                ImageMetadata = metadata
            };
        }
    }
}
