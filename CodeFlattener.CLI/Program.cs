using RepoScribe.Core.Utilities;
using Serilog;
using System.CommandLine;
using System.CommandLine.Invocation;
using RepoScribe.CLI.Commands;
using LibGit2Sharp;
using PdfSharp.Charting;
using PdfSharp.Pdf.Content.Objects;
using RepoScribe.Core.DataModels.Markdown;
using RepoScribe.Core.DataModels;
using RepoScribe.Core.FileHandlers;
using RepoScribe.Core.Helpers;
using RepoScribe.Core.Services;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;

namespace RepoScribe.CLI
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            Logger.Initialize();

            var rootCommand = new RootCommand("RepoScribe - Flatten and document your code repositories");

            rootcommand.addcommand(new FlattenCommand().getcommand());
            rootcommand.addcommand(new IgnoreCommand().getcommand());
            rootcommand.addcommand(new RepoCommand().getcommand());
            rootcommand.addcommand(new ExtractCommand().getcommand());

            return await rootCommand.InvokeAsync(args);
        }
    }
}
Certainly! Let's address each of your requirements step-by-step to enhance your `CodeFlattener` project. We'll:

1. * *Rename the Project to a Unique and Clever Name**
2. **Create a Unified CLI Entry Point**
3. **Develop a Base Data Model for Markdown Output with Custom `ToString` Override**
4. **Incorporate PowerShell Scripts as C# Code**
5. **Segment the CLI into Submodules for Better Organization**
6. **Update Documentation Accordingly**

---

## 1. Rename the Project to a Unique and Clever Name

**Proposed Name:** `RepoScribe`

*Rationale:*The name combines "Repository" and "Scribe" (a writer), reflecting the tool's purpose of flattening and documenting codebases.

### Steps to Rename:

1. **Rename Project Folders:**
   - `CodeFlattener` → `RepoScribe`
   - `CodeFlattener.CLI` → `RepoScribe.CLI`
   - `CodeFlattener.Core` → `RepoScribe.Core`
   - `CodeFlattener.Tests` → `RepoScribe.Tests`

2. **Update `.csproj` Files:**
   -Change `< Project Sdk = "Microsoft.NET.Sdk" >` to reflect the new project names where necessary.

3. * *Update Namespace Declarations:**
   -Replace `CodeFlattener` with `RepoScribe` in all `.cs` files.

### Example:

**Before (`CodeFlattener.Core/FileHandlers/CodeFileHandler.cs`):**
```csharp
namespace CodeFlattener.Core.FileHandlers
{
    // ...
}
```

**After(`RepoScribe.Core / FileHandlers / CodeFileHandler.cs`):**
```csharp
namespace RepoScribe.Core.FileHandlers
{
    // ...
}
```

---

## 2. Create a Unified CLI Entry Point

We'll consolidate the CLI functionalities into a single entry point, ensuring all commands are accessible from one place.

### Steps:

1. * *Create a Unified CLI Project:**
   -Ensure that `RepoScribe.CLI` serves as the main executable.

2. **Update `Program.cs` in `RepoScribe.CLI` to Handle All Commands:**

### Example:

**`RepoScribe.CLI / Program.cs`:**
```csharp
using RepoScribe.Core.Utilities;
using Serilog;
using System.CommandLine;
using System.CommandLine.Invocation;
using RepoScribe.CLI.Commands;

namespace RepoScribe.CLI
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            Logger.Initialize();

            var rootCommand = new RootCommand("RepoScribe - Flatten and document your code repositories");

            // Add subcommands
            rootCommand.AddCommand(new FlattenCommand().GetCommand());
            rootCommand.AddCommand(new IgnoreCommand().GetCommand());
            rootCommand.AddCommand(new RepoCommand().GetCommand());

            return await rootCommand.InvokeAsync(args);
        }
    }
}
```

---

## 3. Develop a Base Data Model for Markdown Output with Custom `ToString` Override

We'll create an abstract `MarkdownContent` class that serves as a base for different markdown elements. It will include a `ToString` override to render markdown content and interface with `IOutputTemplating`.

### Steps:

1. * *Create `MarkdownContent` Abstract Class:**
   -Define common properties and methods.
   - Implement `ToString` for markdown rendering.

2. **Define `IOutputTemplating` Interface:**
   -Allow different output templates to be applied.

3. **Implement Concrete Markdown Classes:**
   -For example, `Header`, `CodeBlock`, etc.

### Example:

**`RepoScribe.Core/DataModels/Markdown/MarkdownContent.cs`:**
```csharp
namespace RepoScribe.Core.DataModels.Markdown
{
    public abstract class MarkdownContent : IOutputTemplating
    {
        public abstract string ToMarkdown();

        public override string ToString()
        {
            return ToMarkdown();
        }

        public abstract string ApplyTemplate(string template);
    }
}
```

**`RepoScribe.Core / DataModels / Markdown / Header.cs`:**
```csharp
namespace RepoScribe.Core.DataModels.Markdown
{
    public class Header : MarkdownContent
    {
        public int Level { get; set; }
        public string Text { get; set; }

        public Header(int level, string text)
        {
            Level = level;
            Text = text;
        }

        public override string ToMarkdown()
        {
            return $"{new string('#', Level)} {Text}\n";
        }

        public override string ApplyTemplate(string template)
        {
            return string.Format(template, ToMarkdown());
        }
    }
}
```

**`RepoScribe.Core / DataModels / Markdown / CodeBlock.cs`:**
```csharp
namespace RepoScribe.Core.DataModels.Markdown
{
    public class CodeBlock : MarkdownContent
    {
        public string Language { get; set; }
        public string Content { get; set; }

        public CodeBlock(string language, string content)
        {
            Language = language;
            Content = content;
        }

        public override string ToMarkdown()
        {
            return $"```{Language}\n{Content}\n```\n";
        }

        public override string ApplyTemplate(string template)
        {
            return string.Format(template, ToMarkdown());
        }
    }
}
```

**`RepoScribe.Core / DataModels / Markdown / MarkdownDocument.cs`:**
```csharp
namespace RepoScribe.Core.DataModels.Markdown
{
    public class MarkdownDocument
    {
        private readonly List<MarkdownContent> _contents = new();

        public void AddContent(MarkdownContent content)
        {
            _contents.Add(content);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var content in _contents)
            {
                sb.Append(content.ToString());
            }
            return sb.ToString();
        }
    }
}
```

---

## 4. Incorporate PowerShell Scripts as C# Code

We'll translate the provided PowerShell scripts into C# functionalities and integrate them into the CLI as commands or services.

### Steps:

1. * *Translate `Extract - Codeblocks.ps1` to C#:**
   - Implement methods to extract code blocks from markdown files.
   - Generate unique IDs based on content hashes.
   - Convert dotted paths to file paths.

2. **Translate `Flatten-All` Script to C#:**
   - Implement methods to process directories, execute flattening, and log progress.

3. **Integrate These Functionalities into CLI Commands:**
   -For example, `repo-scribe extract` and `repo-scribe flatten-all`.

### Example:

**`RepoScribe.Core/Utilities/HashUtility.cs`:**
```csharp
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
```

**`RepoScribe.Core / Utilities / PathUtility.cs`:**
```csharp
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
```

**`RepoScribe.Core/Services/MarkdownExtractor.cs`:**
```csharp
using RepoScribe.Core.DataModels.Markdown;
using System.Text.RegularExpressions;

namespace RepoScribe.Core.Services
{
    public class MarkdownExtractor
    {
        private readonly string _inputDirectory;

        public MarkdownExtractor(string inputDirectory)
        {
            _inputDirectory = inputDirectory;
        }

        public IEnumerable<string> ExtractCodeBlocks()
        {
            var markdownFiles = Directory.GetFiles(_inputDirectory, "*.md", SearchOption.AllDirectories);

            foreach (var file in markdownFiles)
            {
                var content = File.ReadAllText(file);
                var matches = Regex.Matches(content, @"(?ms)^# (.+?)\r?\n```(\w+)\r?\n(.+?)\r?\n```");

                foreach (Match match in matches)
                {
                    var path = PathUtility.ConvertDottedPathToFilePath(match.Groups[1].Value.Trim());
                    var codeContent = match.Groups[3].Value.Trim();
                    var id = HashUtility.GetUniqueId(codeContent);

                    var codeObject = new
                    {
                        Id = id,
                        Path = path,
                        Content = codeContent
                    };

                    yield return Newtonsoft.Json.JsonConvert.SerializeObject(codeObject);
                }
            }
        }
    }
}
```

**`RepoScribe.Core/Services/FlattenAllService.cs`:**
```csharp
using RepoScribe.Core.DataModels.Markdown;
using RepoScribe.Core.Utilities;
using Serilog;

namespace RepoScribe.Core.Services
{
    public class FlattenAllService
    {
        private readonly string _codeFlattenerPath;
        private readonly string _outputDirectory;

        public FlattenAllService(string codeFlattenerPath, string outputDirectory)
        {
            _codeFlattenerPath = codeFlattenerPath;
            _outputDirectory = outputDirectory;
        }

        public async Task FlattenAllAsync()
        {
            if (!File.Exists(_codeFlattenerPath))
            {
                Log.Error($"CodeFlattener.exe not found at {_codeFlattenerPath}");
                return;
            }

            Log.Information("Starting Flatten-All process...");

            var directories = Directory.GetDirectories(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories)
                                      .Where(dir => Directory.Exists(Path.Combine(dir, ".git")))
                                      .ToList();

            int total = directories.Count;
            int processed = 0;
            int success = 0;
            int failed = 0;

            foreach (var dir in directories)
            {
                processed++;
                var baseName = new DirectoryInfo(dir).Name;
                var dottedPath = PathUtility.ConvertFilePathToDottedPath(dir);
                var outputFile = Path.Combine(_outputDirectory, $"{dottedPath}.md");

                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = _codeFlattenerPath,
                            Arguments = $"\"{dir}\" \"{outputFile}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };

                    process.Start();
                    string stdout = await process.StandardOutput.ReadToEndAsync();
                    string stderr = await process.StandardError.ReadToEndAsync();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        Log.Information($"Successfully processed {baseName}. Output saved to {outputFile}");
                        success++;
                    }
                    else
                    {
                        Log.Error($"Error processing {baseName}. Exit code: {process.ExitCode}");
                        Log.Error($"Error details: {stderr}");
                        failed++;
                    }

                    double percentComplete = Math.Round((double)processed / total * 100, 2);
                    Log.Information($"Progress: {percentComplete}% ({processed}/{total})");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Failed to process directory {dir}");
                    failed++;
                }
            }

            Log.Information("Flatten-All process complete.");
            Log.Information($"Total directories processed: {processed}");
            Log.Information($"Successful: {success}");
            Log.Information($"Failed: {failed}");
        }
    }
}
```

---

## 5. Segment the CLI into Submodules for Better Organization

We'll organize the CLI commands into submodules for clarity and maintainability.

### Steps:

1. **Create a `Commands` Folder in `RepoScribe.CLI`:**
   - Add separate classes for each command category (e.g., `FlattenCommand`, `IgnoreCommand`, `RepoCommand`).

2. **Implement Each Command as a Separate Class:**

### Example:

**`RepoScribe.CLI/Commands/FlattenCommand.cs`:**
```csharp
using System.CommandLine;
using System.CommandLine.Invocation;
using RepoScribe.Core.DataModels.Markdown;
using RepoScribe.Core.Helpers;
using RepoScribe.Core.Utilities;
using RepoScribe.Core.Services;
using Serilog;

namespace RepoScribe.CLI.Commands
{
    public class FlattenCommand
    {
        public Command GetCommand()
        {
            var command = new Command("flatten", "Flatten a codebase into a Markdown document");

            var inputOption = new Option<string>(
                new[] { "--input", "-i" },
                description: "The root directory to process")
            { IsRequired = true };

            var outputOption = new Option<string>(
                new[] { "--output", "-o" },
                description: "The output Markdown file. Default: {input_basename}.md");

            var compressOption = new Option<bool>(
                new[] { "--compress", "-c" },
                getDefaultValue: () => false,
                description: "Enable content compression");

            command.AddOption(inputOption);
            command.AddOption(outputOption);
            command.AddOption(compressOption);

            command.Handler = CommandHandler.Create<string, string, bool>(HandleFlatten);

            return command;
        }

        private void HandleFlatten(string input, string output, bool compress)
        {
            try
            {
                if (string.IsNullOrEmpty(output))
                {
                    output = Path.Combine(input, Path.GetFileName(input) + ".md");
                }

                var configManager = new ConfigurationManager("appsettings.json");
                var languageMap = configManager.GetLanguageMap();
                var ignoredPaths = configManager.GetIgnoredPaths();

                var fileHandlers = new List<IFileHandler>
                {
                    new CodeFileHandler(languageMap),
                    new ImageFileHandler(),
                    new PdfFileHandler(),
                    new SqliteFileHandler()
                };

                var fileHelper = new FileHelper(fileHandlers);

                input = Path.GetFullPath(input);
                output = Path.GetFullPath(output);

                Log.Information($"Input Path: {input}");
                Log.Information($"Output Path: {output}");
                Log.Information($"Compress: {compress}");

                input = Environment.ExpandEnvironmentVariables(input);
                output = Environment.ExpandEnvironmentVariables(output);

                if (!Directory.Exists(input))
                {
                    Log.Error($"The input directory {input} does not exist.");
                    return;
                }

                var files = Directory.EnumerateFiles(input, "*", SearchOption.AllDirectories)
                    .Where(file => !ignoredPaths.Any(ignored =>
                        file.Contains(Path.DirectorySeparatorChar + ignored + Path.DirectorySeparatorChar) ||
                        file.Contains(Path.DirectorySeparatorChar + ignored + Path.AltDirectorySeparatorChar) ||
                        file.EndsWith(Path.DirectorySeparatorChar + ignored) ||
                        file.EndsWith(Path.AltDirectorySeparatorChar + ignored)))
                    .ToList();

                Log.Information($"Found {files.Count} files to process.");

                var markdownContent = new MarkdownDocument();

                markdownContent.AddContent(new Header(1, Path.GetFileName(input)));

                foreach (var filePath in files)
                {
                    try
                    {
                        var fileMetadata = fileHelper.ProcessFile(filePath);
                        if (fileMetadata != null)
                        {
                            AppendFileContent(markdownContent, input, fileMetadata, compress);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, $"Failed to process file {filePath}. Skipping.");
                    }
                }

                File.WriteAllText(output, markdownContent.ToString());
                Log.Information($"Output written to {output}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while flattening the codebase.");
            }
            finally
            {
                Logger.CloseAndFlush();
            }
        }

        private void AppendFileContent(
            MarkdownDocument markdownContent,
            string rootFolder,
            FileMetadata fileMetadata,
            bool compress)
        {
            var relativePath = Path.GetRelativePath(rootFolder, fileMetadata.Path);
            markdownContent.AddContent(new Header(2, relativePath.Replace('\\', '/')));
            var content = compress ? CompressContent(fileMetadata.Content) : fileMetadata.Content;
            markdownContent.AddContent(new CodeBlock(fileMetadata.Language, content));
        }

        private string CompressContent(string content)
        {
            // Implement compression logic as needed
            return content.Replace(" ", string.Empty);
        }
    }
}
```

**`RepoScribe.CLI/Commands/IgnoreCommand.cs`:**
```csharp
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
```

**`RepoScribe.CLI/Commands/RepoCommand.cs`:**
```csharp
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
```

**`RepoScribe.CLI/Commands/ExtractCommand.cs`:**
```csharp
using System.CommandLine;
using System.CommandLine.Invocation;
using RepoScribe.Core.Services;
using Serilog;

namespace RepoScribe.CLI.Commands
{
    public class ExtractCommand
    {
        public Command GetCommand()
        {
            var command = new Command("extract", "Extract code blocks from Markdown files");

            var configPathOption = new Option<string>(
                new[] { "--config", "-c" },
                description: "Path to the configuration file",
                getDefaultValue: () => "config.json");

            command.AddOption(configPathOption);

            command.Handler = CommandHandler.Create<string>(HandleExtract);

            return command;
        }

        private void HandleExtract(string config)
        {
            try
            {
                var configManager = new ConfigurationManager(config);
                var inputDirectory = configManager.GetExtractChunksInputDirectory();

                if (string.IsNullOrEmpty(inputDirectory) || !Directory.Exists(inputDirectory))
                {
                    Log.Error($"Invalid input directory: {inputDirectory}");
                    return;
                }

                var extractor = new MarkdownExtractor(inputDirectory);
                var codeBlocks = extractor.ExtractCodeBlocks();

                foreach (var codeBlock in codeBlocks)
                {
                    Console.WriteLine(codeBlock);
                }

                Log.Information("Code blocks extraction completed.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to extract code blocks.");
            }
        }
    }
}
```

**Update `Program.cs` to Include `ExtractCommand`:**
```csharp
// Inside Program.cs
rootCommand.AddCommand(new FlattenCommand().GetCommand());
rootCommand.AddCommand(new IgnoreCommand().GetCommand());
rootCommand.AddCommand(new RepoCommand().GetCommand());
rootCommand.AddCommand(new ExtractCommand().GetCommand());
```

---

## 6. Update Documentation Accordingly

Ensure that the documentation reflects the new project structure, commands, and functionalities.

### Steps:

1. **Update README.md:**
   - Rename project references from `CodeFlattener` to `RepoScribe`.
   - Document new commands and their usage.
   - Provide examples for each command.

2. **Add Help Commands:**
   - Utilize `System.CommandLine` to provide help descriptions for each command and option.

### Example:

**`README.md`:**
```markdown


---

## Final Project Structure

```
RepoScribe/
├── RepoScribe.CLI/
│   ├── Commands/
│   │   ├── FlattenCommand.cs
│   │   ├── IgnoreCommand.cs
│   │   ├── RepoCommand.cs
│   │   └── ExtractCommand.cs
│   ├── Program.cs
│   └── RepoScribe.CLI.csproj
├── RepoScribe.Core/
│   ├── DataModels/
│   │   ├── FileMetadata.cs
│   │   ├── LineContent.cs
│   │   ├── RepositoryMetadata.cs
│   │   └── Markdown/
│   │       ├── MarkdownContent.cs
│   │       ├── Header.cs
│   │       ├── CodeBlock.cs
│   │       └── MarkdownDocument.cs
│   ├── FileHandlers/
│   │   ├── CodeFileHandler.cs
│   │   ├── IFileHandler.cs
│   │   ├── ImageFileHandler.cs
│   │   ├── PdfFileHandler.cs
│   │   └── SqliteFileHandler.cs
│   ├── Helpers/
│   │   ├── FileHelper.cs
│   │   └── GitHelper.cs
│   ├── Services/
│   │   ├── MarkdownExtractor.cs
│   │   └── FlattenAllService.cs
│   ├── Utilities/
│   │   ├── ConfigurationManager.cs
│   │   ├── Logger.cs
│   │   ├── RepositoryManager.cs
│   │   ├── HashUtility.cs
│   │   └── PathUtility.cs
│   └── RepoScribe.Core.csproj
├── RepoScribe.Tests/
│   ├── RepoScribe.Tests.csproj
│   └── RepoScribeTests.cs
├── appsettings.json
├── config.json
└── README.md
```

---

## Additional Recommendations

1. **Dependency Injection:**
   - Consider using dependency injection (e.g., `Microsoft.Extensions.DependencyInjection`) to manage dependencies, especially for services and handlers.

2. **Asynchronous Operations:**
   - Ensure that long-running tasks (like cloning repositories or processing large codebases) are asynchronous to improve performance and responsiveness.

3. **Error Handling and Validation:**
   - Implement comprehensive error handling and validate user inputs to enhance reliability.

4. **Extensibility:**
   - Design the system to be easily extensible, allowing new file handlers or features to be added with minimal changes.

5. **Unit Tests:**
   - Expand the unit tests to cover all new functionalities and edge cases to ensure robustness.

---

By following the above steps, your `RepoScribe` tool will be more organized, feature-rich, and user-friendly. This setup not only addresses the immediate requirements but also lays a solid foundation for future enhancements.