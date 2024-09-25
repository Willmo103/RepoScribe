using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RepoScribe.Core.Utilities
{
    public class RepositoryManager
    {
        private readonly string _configPath;
        public List<string> Repositories { get; private set; }

        public RepositoryManager(string configPath)
        {
            _configPath = configPath;
            if (File.Exists(_configPath))
            {
                var json = File.ReadAllText(_configPath);
                Repositories = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();
            }
            else
            {
                Repositories = new List<string>();
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(Repositories, Formatting.Indented);
            File.WriteAllText(_configPath, json);
        }
    }
}
