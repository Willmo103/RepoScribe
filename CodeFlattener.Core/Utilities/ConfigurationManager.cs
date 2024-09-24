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
                .ToDictionary(x => x.Key, x => x.Value ?? "");
        }

        public List<string> GetIgnoredPaths()
        {
            return _configuration.GetSection("Ignored")
                .GetChildren()
                .Select(x => x.Key)
                .ToList();
        }

        // Profiles Will be implemented in a future version. 
        // Control for specific configurations for scanning different directories

        // Potential Profiles: 

        // 1. Default: Scan all files, no exclusions
        // 2. BuildIgnore: Generates list of extensions to ignore by attempting to read files
        //    And adding them to the JSON file as they are unreadable
        // 3. Custom: User-defined profile: Create a specific list of extensions to scan or ignore load a profile by name

        //public List<Dictionary<string, string>> GetProfiles()
        //{
        //    return _configuration.GetSection("Profiles")
        //        .GetChildren()
        //        .Select(x => x.GetChildren().ToDictionary(y => y.Key, y => y.Value))
        //        .ToList();
        //}
    }
}
