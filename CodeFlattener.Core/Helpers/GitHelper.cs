using RepoScribe.Core.DataModels;
using LibGit2Sharp;

namespace RepoScribe.Core.Helpers
{
    public class GitHelper
    {
        public RepositoryMetadata GetRepositoryMetadata(string repoPath)
        {
            using var repo = new Repository(repoPath);

            var metadata = new RepositoryMetadata
            {
                Url = repo.Network.Remotes.FirstOrDefault()?.Url ?? "",
                Author = repo.Commits.FirstOrDefault()?.Author.Name ?? "",
                Readme = GetReadmeContent(repoPath)
            };

            return metadata;
        }

        private string GetReadmeContent(string repoPath)
        {
            var readmePath = Directory.GetFiles(repoPath, "README.*", SearchOption.TopDirectoryOnly)
                .FirstOrDefault();

            return readmePath != null ? File.ReadAllText(readmePath) : string.Empty;
        }
    }
}
