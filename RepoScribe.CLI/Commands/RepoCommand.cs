using System.CommandLine;
using System.CommandLine.Invocation;
using RepoScribe.Core.Utilities;
using LibGit2Sharp;
using Serilog;

namespace RepoScribe.CLI.Commands
{
    public class RepoCommand
    {
        public Command GetCommand()
        {
            var command = new Command("repo", "Manage repositories");

            var addRepoCommand = new Command("add", "Add a repository")
            {
                new Argument<string>("url", "The Git repository URL")
            };
            addRepoCommand.Handler = CommandHandler.Create<string>(HandleAddRepo);

            var listRepoCommand = new Command("list", "List repositories");
            listRepoCommand.Handler = CommandHandler.Create(HandleListRepo);

            var removeRepoCommand = new Command("remove", "Remove a repository")
            {
                new Argument<string>("url", "The Git repository URL to remove")
            };
            removeRepoCommand.Handler = CommandHandler.Create<string>(HandleRemoveRepo);

            var cloneRepoCommand = new Command("clone", "Clone a repository")
            {
                new Argument<string>("url", "The Git repository URL to clone"),
                new Option<string>(new[] { "--path", "-p" }, "The local path to clone the repository into")
            };
            cloneRepoCommand.Handler = CommandHandler.Create<string, string>(HandleCloneRepo);

            command.AddCommand(addRepoCommand);
            command.AddCommand(listRepoCommand);
            command.AddCommand(removeRepoCommand);
            command.AddCommand(cloneRepoCommand);

            return command;
        }

        private void HandleAddRepo(string url)
        {
            try
            {
                var configPath = "repositories.json";
                var repoManager = new RepositoryManager(configPath);

                if (!repoManager.Repositories.Contains(url, StringComparer.OrdinalIgnoreCase))
                {
                    repoManager.Repositories.Add(url);
                    repoManager.Save();
                    Log.Information($"Added repository '{url}'.");
                }
                else
                {
                    Log.Information($"Repository '{url}' is already in the list.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to add repository.");
            }
        }

        private void HandleListRepo()
        {
            try
            {
                var repoManager = new RepositoryManager("repositories.json");
                Log.Information("Repositories:");
                foreach (var repo in repoManager.Repositories)
                {
                    Console.WriteLine($"- {repo}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to list repositories.");
            }
        }

        private void HandleRemoveRepo(string url)
        {
            try
            {
                var repoManager = new RepositoryManager("repositories.json");

                if (repoManager.Repositories.Remove(url))
                {
                    repoManager.Save();
                    Log.Information($"Removed repository '{url}'.");
                }
                else
                {
                    Log.Information($"Repository '{url}' was not found in the list.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to remove repository.");
            }
        }

        private void HandleCloneRepo(string url, string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(url));
                }

                Repository.Clone(url, path);
                Log.Information($"Cloned repository '{url}' to '{path}'.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to clone repository '{url}'.");
            }
        }
    }
}
