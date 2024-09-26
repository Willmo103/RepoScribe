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
