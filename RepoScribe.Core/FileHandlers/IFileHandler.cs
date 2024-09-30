using RepoScribe.Core.DataModels;
using System;

namespace RepoScribe.Core.FileHandlers
{
    [Obsolete("IFileHandler is deprecated. Use IContentExtractor instead.")]
    public interface IFileHandler
    {
        bool CanHandle(string extension);
        FileMetadata ProcessFile(string filePath);
    }
}
