using System.CommandLine;
using System.CommandLine.Invocation;
using RepoScribe.Core.Utilities;
using Serilog;

namespace RepoScribe.CLI.Commands
{
    public class IgnoreCommand
    {
        public Command GetCommand()
        {
            var command = new Command("ignore", "Manage ignored paths");

            var addCommand = new Command("add", "Add a path to the ignore list")
            {
                new Argument<string>("path", "The path to ignore")
            };
            addCommand.Handler = CommandHandler.Create<string>(HandleAddIgnore);

            var listCommand = new Command("list", "List ignored paths");
            listCommand.Handler = CommandHandler.Create(HandleListIgnore);

            var removeCommand = new Command("remove", "Remove a path from the ignore list")
            {
                new Argument<string>("path", "The path to remove")
            };
            removeCommand.Handler = CommandHandler.Create<string>(HandleRemoveIgnore);

            command.AddCommand(addCommand);
            command.AddCommand(listCommand);
            command.AddCommand(removeCommand);

            return command;
        }

        private void HandleAddIgnore(string path)
        {
            try
            {
                var configPath = "appsettings.json";
                var config = new ConfigurationManager(configPath);
                var ignoredPaths = config.GetIgnoredPaths();

                if (!ignoredPaths.Contains(path, StringComparer.OrdinalIgnoreCase))
                {
                    ignoredPaths.Add(path);
                    SaveIgnoredPaths(configPath, ignoredPaths);
                    Log.Information($"Added '{path}' to ignore list.");
                }
                else
                {
                    Log.Information($"'{path}' is already in the ignore list.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to add ignore path.");
            }
        }

        private void HandleListIgnore()
        {
            try
            {
                var config = new ConfigurationManager("appsettings.json");
                var ignoredPaths = config.GetIgnoredPaths();
                Log.Information("Ignored Paths:");
                foreach (var path in ignoredPaths)
                {
                    Console.WriteLine($"- {path}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to list ignore paths.");
            }
        }

        private void HandleRemoveIgnore(string path)
        {
            try
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
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to remove ignore path.");
            }
        }

        private void SaveIgnoredPaths(string configPath, List<string> ignoredPaths)
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
