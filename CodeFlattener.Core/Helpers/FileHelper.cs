using CodeFlattener.Core.DataModels;
using CodeFlattener.Core.FileHandlers;

namespace CodeFlattener.Core.Helpers
{
    public class FileHelper
    {
        private readonly List<IFileHandler> _fileHandlers;

        public FileHelper(List<IFileHandler> fileHandlers)
        {
            _fileHandlers = fileHandlers;
        }

        public FileMetadata ProcessFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            var handler = _fileHandlers.FirstOrDefault(h => h.CanHandle(extension));

            if (handler != null)
            {
                return handler.ProcessFile(filePath);
            }

            return null;
        }
    }
}
