using CodeFlattener.Core.Utilities;
using Serilog;
using System.CommandLine;
using LibGit2Sharp;

namespace CodeFlattener.CLI
{
    class Program
    {
        static int Main(string[] args)
        {
            Logger.Initialize();

            var rootCommand = new RootCommand("CodeFlattener CLI");

            // Ignore Commands
            var ignoreCommand = new Command("ignore", "Manage ignored paths");
            var addIgnoreCommand = new Command("add", "Add a path to the ignore list")
            {
                new Argument<string>("path", "The path to ignore")
            };
            var listIgnoreCommand = new Command("list", "List ignored paths");
            var removeIgnoreCommand = new Command("remove", "Remove a path from the ignore list")
            {
                new Argument<string>("path", "The path to remove")
            };

            ignoreCommand.AddCommand(addIgnoreCommand);
            ignoreCommand.AddCommand(listIgnoreCommand);
            ignoreCommand.AddCommand(removeIgnoreCommand);

            // Repo Commands
            var repoCommand = new Command("repo", "Manage repositories");
            var addRepoCommand = new Command("add", "Add a repository")
            {
                new Argument<string>("url", "The Git repository URL")
            };
            var listRepoCommand = new Command("list", "List repositories");
            var removeRepoCommand = new Command("remove", "Remove a repository")
            {
                new Argument<string>("url", "The Git repository URL to remove")
            };
            var cloneRepoCommand = new Command("clone", "Clone a repository")
            {
                new Argument<string>("url", "The Git repository URL to clone"),
                new Option<string>(new[] { "--path", "-p" }, "The local path to clone the repository into")
            };

            repoCommand.AddCommand(addRepoCommand);
            repoCommand.AddCommand(listRepoCommand);
            repoCommand.AddCommand(removeRepoCommand);
            repoCommand.AddCommand(cloneRepoCommand);

            rootCommand.AddCommand(ignoreCommand);
            rootCommand.AddCommand(repoCommand);

            // Handlers for Ignore Commands
            addIgnoreCommand.SetHandler((string path) =>
            {
                var configPath = "appsettings.json";
                var config = new ConfigurationManager(configPath);
                var ignoredPaths = config.GetIgnoredPaths();

                if (!ignoredPaths.Contains(path, StringComparer.OrdinalIgnoreCase))
                {
                    ignoredPaths.Add(path);
                    // Save back to appsettings.json
                    SaveIgnoredPaths(configPath, ignoredPaths);
                    Log.Information($"Added '{path}' to ignore list.");
                }
                else
                {
                    Log.Information($"'{path}' is already in the ignore list.");
                }
            }, addIgnoreCommand.Arguments[0]);

            listIgnoreCommand.SetHandler(() =>
            {
                var config = new ConfigurationManager("appsettings.json");
                var ignoredPaths = config.GetIgnoredPaths();
                Log.Information("Ignored Paths:");
                foreach (var path in ignoredPaths)
                {
                    Console.WriteLine($"- {path}");
                }
            });

            removeIgnoreCommand.SetHandler((string path) =>
            {
                var configPath = "appsettings.json";
                var config = new ConfigurationManager(configPath);
                var ignoredPaths = config.GetIgnoredPaths();

                if (ignoredPaths.Remove(path))
                {
                    SaveIgnoredPaths(configPath, ignoredPaths);
                    Log.Information($"Removed '{path}' from ignore list.");
                }
                else
                {
                    Log.Information($"'{path}' was not found in the ignore list.");
                }
            }, removeIgnoreCommand.Arguments[0]);

            // Handlers for Repo Commands
            addRepoCommand.SetHandler((string url) =>
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
            }, addRepoCommand.Arguments[0]);

            listRepoCommand.SetHandler(() =>
            {
                var repoManager = new RepositoryManager("repositories.json");
                Log.Information("Repositories:");
                foreach (var repo in repoManager.Repositories)
                {
                    Console.WriteLine($"- {repo}");
                }
            });

            removeRepoCommand.SetHandler((string url) =>
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
            }, removeRepoCommand.Arguments[0]);

            cloneRepoCommand.SetHandler((string url, string path) =>
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(url));
                }

                try
                {
                    Repository.Clone(url, path);
                    Log.Information($"Cloned repository '{url}' to '{path}'.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Failed to clone repository '{url}'.");
                }
            }, cloneRepoCommand.Arguments[0], cloneRepoCommand.Options.First());

            return rootCommand.InvokeAsync(args).Result;
        }

        private static void SaveIgnoredPaths(string configPath, List<string> ignoredPaths)
        {
            var configJson = File.ReadAllText(configPath);
            dynamic configData = Newtonsoft.Json.JsonConvert.DeserializeObject(configJson);

            configData.Ignored = new Newtonsoft.Json.Linq.JObject();
            foreach (var path in ignoredPaths)
            {
                configData.Ignored[path] = "User Ignored Path";
            }

            var updatedJson = Newtonsoft.Json.JsonConvert.SerializeObject(configData, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(configPath, updatedJson);
        }
    }
}
