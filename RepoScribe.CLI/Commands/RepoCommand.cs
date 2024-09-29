using RepoScribe.Core.Utilities;
using LibGit2Sharp;
using Serilog;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;

namespace RepoScribe.CLI.Commands
{
    public class RepoCommand : BaseCommand
    {
        public override Command GetCommand()
        {
            var command = new Command("repo", "Manage repositories");

            var addRepoCommand = new Command("add", "Add a repository")
                {
                    new Argument<string>("url", "The Git repository URL")
                };
            addRepoCommand.SetHandler((string url, InvocationContext context) =>
                HandleAddRepo(url, context),
                addRepoCommand.Arguments[0]);

            var listRepoCommand = new Command("list", "List repositories");

            listRepoCommand.SetHandler(async (InvocationContext context) =>
                await HandleListRepo(context));


            var removeRepoCommand = new Command("remove", "Remove a repository")
                {
                    new Argument<string>("url", "The Git repository URL to remove")
                };
            removeRepoCommand.SetHandler((string url, InvocationContext context) =>
                HandleRemoveRepo(url, context),
                removeRepoCommand.Arguments[0]);

            var cloneRepoCommand = new Command("clone", "Clone a repository")
                {
                    new Argument<string>("url", "The Git repository URL to clone"),
                    new Option<string>(new[] { "--path", "-p" }, "The local path to clone the repository into")
                };
            cloneRepoCommand.SetHandler((string url, string path, InvocationContext context) =>
                HandleCloneRepo(url, path, context),
                cloneRepoCommand.Arguments[0],
                cloneRepoCommand.Options[0]);

            command.AddCommand(addRepoCommand);
            command.AddCommand(listRepoCommand);
            command.AddCommand(removeRepoCommand);
            command.AddCommand(cloneRepoCommand);

            return command;
        }

        private void HandleAddRepo(string url, InvocationContext context)
        {
            var console = context.Console;
            var cancellationToken = context.GetCancellationToken();

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

                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to add repository.");
                context.ExitCode = 1;
            }
        }

        private async Task HandleListRepo(InvocationContext context)
        {
            var console = context.Console;
            var cancellationToken = context.GetCancellationToken();

            try
            {
                var repoManager = new RepositoryManager("repositories.json");
                Log.Information("Repositories:");
                foreach (var repo in repoManager.Repositories)
                {
                    console.Out.WriteLine($"- {repo}");
                }

                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to list repositories.");
                context.ExitCode = 1;
            }
        }

        private void HandleRemoveRepo(string url, InvocationContext context)
        {
            var console = context.Console;
            var cancellationToken = context.GetCancellationToken();

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

                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to remove repository.");
                context.ExitCode = 1;
            }
        }

        private void HandleCloneRepo(string url, string path, InvocationContext context)
        {
            var console = context.Console;
            var cancellationToken = context.GetCancellationToken();

            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(url));
                }

                Repository.Clone(url, path);
                Log.Information($"Cloned repository '{url}' to '{path}'.");
                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to clone repository '{url}'.");
                context.ExitCode = 1;
            }
        }
    }
}
