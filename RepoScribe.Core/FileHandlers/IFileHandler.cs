using RepoScribe.Core.DataModels;

namespace RepoScribe.Core.FileHandlers
{
    public interface IFileHandler
    {
        bool CanHandle(string extension);
        FileMetadata ProcessFile(string filePath);
    }
}
