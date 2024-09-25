using RepoScribe.Core.DataModels;
using RepoScribe.Core.FileHandlers;

namespace RepoScribe.Core.Helpers
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
            // If there is no file extension we still need to compare it for cases like
            // .gitignore, .editorconfig, Dockerfile, .env, etc.
            var _extension = Path.GetExtension(filePath).ToLower();
            if (_extension == string.Empty)
            {
                _extension = Path.GetFileName(filePath).ToLower();
            }
            var handler = _fileHandlers.FirstOrDefault(h => h.CanHandle(_extension));

            if (handler != null)
            {
                return handler.ProcessFile(filePath);
            }

            return null;
        }
    }
}
