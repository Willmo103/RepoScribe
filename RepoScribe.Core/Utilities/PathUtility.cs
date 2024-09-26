using System.IO;

namespace RepoScribe.Core.Utilities
{
    public static class PathUtility
    {
        public static string ConvertDottedPathToFilePath(string dottedPath)
        {
            return dottedPath.Replace('.', Path.DirectorySeparatorChar);
        }

        public static string ConvertFilePathToDottedPath(string filePath)
        {
            return filePath.Replace(Path.DirectorySeparatorChar, '.').Replace(Path.AltDirectorySeparatorChar, '.');
        }
    }
}
