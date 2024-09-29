using System.CommandLine;
using RepoScribe.Core.Utilities;
using Serilog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.CommandLine.Invocation;
using System.CommandLine.IO;

namespace RepoScribe.CLI.Commands
{
    public class IgnoreCommand : BaseCommand
    {
        public override Command GetCommand()
        {
            var command = new Command("ignore", "Manage ignored paths");

            var addCommand = new Command("add", "Add a path to the ignore list")
                {
                    new Argument<string>("path", "The path to ignore")
                };
            addCommand.SetHandler((string path, InvocationContext context) =>
                HandleAddIgnore(path, context),
                addCommand.Arguments[0]);

            var listCommand = new Command("list", "List ignored paths");
            listCommand.SetHandler(async (InvocationContext context) =>
                await HandleListIgnore(context));

            var removeCommand = new Command("remove", "Remove a path from the ignore list")
                {
                    new Argument<string>("path", "The path to remove")
                };
            removeCommand.SetHandler((string path, InvocationContext context) =>
                HandleRemoveIgnore(path, context),
                removeCommand.Arguments[0]);

            command.AddCommand(addCommand);
            command.AddCommand(listCommand);
            command.AddCommand(removeCommand);

            return command;
        }

        private void HandleAddIgnore(string path, InvocationContext context)
        {
            var console = context.Console;
            var cancellationToken = context.GetCancellationToken();

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

                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to add ignore path.");
                context.ExitCode = 1;
            }
        }

        private async Task HandleListIgnore(InvocationContext context)
        {
            var console = context.Console;
            var cancellationToken = context.GetCancellationToken();

            try
            {
                var config = new ConfigurationManager("appsettings.json");
                var ignoredPaths = config.GetIgnoredPaths();
                Log.Information("Ignored Paths:");
                foreach (var path in ignoredPaths)
                {
                    console.Out.WriteLine($"- {path}");
                }

                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to list ignore paths.");
                context.ExitCode = 1;
            }
        }

        private void HandleRemoveIgnore(string path, InvocationContext context)
        {
            var console = context.Console;
            var cancellationToken = context.GetCancellationToken();

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

                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to remove ignore path.");
                context.ExitCode = 1;
            }
        }

        private void SaveIgnoredPaths(string configPath, List<string> ignoredPaths)
        {
            var configJson = File.ReadAllText(configPath);
            dynamic configData = JsonConvert.DeserializeObject(configJson);

            configData.Ignored = new JObject();
            foreach (var path in ignoredPaths)
            {
                configData.Ignored[path] = "User Ignored Path";
            }

            var updatedJson = JsonConvert.SerializeObject(configData, Formatting.Indented);
            File.WriteAllText(configPath, updatedJson);
        }
    }
}
