using RepoScribe.Core.DataModels;
using SixLabors.ImageSharp;

namespace RepoScribe.Core.FileHandlers
{
    public class ImageFileHandler : IFileHandler
    {
        private readonly List<string> _supportedExtensions = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

        public bool CanHandle(string extension)
        {
            return _supportedExtensions.Contains(extension.ToLower());
        }

        public FileMetadata ProcessFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            using var image = Image.Load(filePath);
            var metadata = image.Metadata;

            // Extract metadata as needed

            return new FileMetadata
            {
                Path = filePath,
                Owner = fileInfo.GetAccessControl()
                    .GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(),
                LastModified = fileInfo.LastWriteTime,
                SizeMB = fileInfo.Length / (1024.0 * 1024.0),
                Language = "image",
                Content = null, // Binary content is not included
                Lines = null
            };
        }
    }
}
