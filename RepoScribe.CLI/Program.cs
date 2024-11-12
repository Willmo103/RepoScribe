using System.CommandLine;
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
            // Initialize the logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var rootCommand = new RootCommand("RepoScribe - Flatten and document your code repositories");

            rootCommand.AddCommand(new FlattenCommand().GetCommand());
            rootCommand.AddCommand(new IgnoreCommand().GetCommand());
            rootCommand.AddCommand(new RepoCommand().GetCommand());
            rootCommand.AddCommand(new ExtractCommand().GetCommand());

            return await rootCommand.InvokeAsync(args);
        }
    }
}
