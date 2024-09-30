using LibGit2Sharp;
using Microsoft.Extensions.Configuration;

namespace RepoScribe.Core.Utilities
{
    public class ConfigurationManager
    {
        private readonly IConfiguration _configuration;
        // Default configuration paths for the application

        // If the user has a configuration file in their home directory, use that
        protected string _homeDirConfigPath = "%USERPROFILE%\\Documents\\RepoScribe\\appsettings.json";

        // If the user has a configuration file in the application directory, use that
        protected string _defaultConfigPath = "appsettings.json";

        // If the path is set as an environment variable, use that
        protected string _envVarConfigPath = System.Environment.GetEnvironmentVariable("REPOSCRIBE_CONFIG") ?? "";

        protected ConfigurationManager()
        {
            try
            {

                if (System.IO.File.Exists(_homeDirConfigPath))
                {
                    _configuration = new ConfigurationBuilder()
                        .AddJsonFile(_homeDirConfigPath, optional: true)
                        .Build();
                }
                else if (System.IO.File.Exists(_defaultConfigPath))
                {
                    _configuration = new ConfigurationBuilder()
                        .AddJsonFile(_defaultConfigPath, optional: true)
                        .Build();
                }
                else if (System.IO.File.Exists(_envVarConfigPath))
                {
                    _configuration = new ConfigurationBuilder()
                        .AddJsonFile(_envVarConfigPath, optional: true)
                        .Build();
                }
                else
                {
                    _configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true)
                        .Build();
                }
            } catch (System.IO.FileNotFoundException)
            {
                throw new System.IO.FileNotFoundException("No configuration file found. Please create an appsettings.json file in the application directory or in your home directory.");
            }

        }

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

        public string GetExtractChunksInputDirectory()
        {
            return _configuration.GetSection("ExtractChunksInputDirectory").Value ?? "%USERPROFILE%\\Documents\\Codeblocks";
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
