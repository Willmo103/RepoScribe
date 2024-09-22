using CodeFlattener.Core.DataModels;

namespace CodeFlattener.Core.FileHandlers
{
    public interface IFileHandler
    {
        bool CanHandle(string extension);
        FileMetadata ProcessFile(string filePath);
    }
}
