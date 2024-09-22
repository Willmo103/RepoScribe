using LibGit2Sharp;
using Microsoft.Extensions.Configuration;

namespace CodeFlattener.Core.Utilities
{
    public class ConfigurationManager
    {
        private readonly IConfiguration _configuration;

        public ConfigurationManager(string configPath)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath, optional: true)
                .Build();
        }

        public Dictionary<string, string> GetLanguageMap()
        {
            return _configuration.GetSection("AllowedFiles")
                .GetChildren()
                .ToDictionary(x => x.Key, x => x.Value ?? "" );
        }

        public List<string> GetIgnoredPaths()
        {
            return _configuration.GetSection("Ignored")
                .GetChildren()
                .Select(x => x.Key)
                .ToList();
        }
    }
}
