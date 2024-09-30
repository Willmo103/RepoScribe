using System.CommandLine;
using System.CommandLine.Invocation;

namespace RepoScribe.CLI.Commands
{
    public abstract class BaseCommand
    {
        public abstract Command GetCommand();
    }
}
