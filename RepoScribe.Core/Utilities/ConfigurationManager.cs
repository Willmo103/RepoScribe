using Microsoft.Extensions.Configuration;

namespace RepoScribe.Core.Utilities
{
    public class ConfigurationManager
    {
        private readonly IConfiguration _configuration;
        // Default configuration paths for the application

        // If the user has a configuration file in their home directory, use that
        protected string _homeDirConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RepoScribe", "appsettings.json");

        // If the user has a configuration file in the application directory, use that
        protected string _defaultConfigPath = "appsettings.json";

        // If the path is set as an environment variable, use that
        protected string _envVarConfigPath = Environment.GetEnvironmentVariable("REPOSCRIBE_CONFIG") ?? "";

        public ConfigurationManager()
        {
            try
            {
                var builder = new ConfigurationBuilder();

                if (File.Exists(_envVarConfigPath))
                {
                    builder.AddJsonFile(_envVarConfigPath, optional: true);
                }

                if (File.Exists(_homeDirConfigPath))
                {
                    builder.AddJsonFile(_homeDirConfigPath, optional: true);
                }

                if (File.Exists(_defaultConfigPath))
                {
                    builder.AddJsonFile(_defaultConfigPath, optional: true);
                }

                _configuration = builder.Build();
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("No configuration file found. Please create an appsettings.json file in the application directory or in your home directory.");
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
            return _configuration.GetSection("ExtractChunksInputDirectory").Value ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Codeblocks");
        }

        // Future implementations for Profiles can be added here
    }
}
