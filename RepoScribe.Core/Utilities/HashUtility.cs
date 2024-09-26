using System.Security.Cryptography;
using System.Text;

namespace RepoScribe.Core.Utilities
{
    public static class HashUtility
    {
        public static string GetContentHash(string content)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(content);
            var hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public static string GetUniqueId(string content)
        {
            var hash = GetContentHash(content);
            return hash.Substring(0, 32); // First 32 characters
        }
    }
}
