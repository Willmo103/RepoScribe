using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace DataModelLib
{
    public class PGData
    {
        public PGData() { }
        public PGData(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            else if (path == "")
            {
                throw new ArgumentException("Path cannot be empty", nameof(path));
            }
           
            if (!Path.Exists(path))
            {
                throw new FileNotFoundException("File not found", path);
            }
            else if (path.Contains("appsettings.json") || path.Contains(".json"))
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(path, optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
            }
            else
            {
                try
                {
                    var confPath = Path.Combine(path + "appsettings.json");
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(confPath, optional: true, reloadOnChange: true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: ");
                    Console.WriteLine(ex.ToString());

                    try
                    {

                    var confPath = Path.Combine(path + ".json");
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(confPath, optional: true, reloadOnChange: true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: ");
                        Console.WriteLine(ex.ToString());

                    }
                }
        }

        //public void GetConnectionString(string name)
        //{
        //    if (name == null)
        //    {
        //        throw new ArgumentNullException(nameof(name));
        //    }
        //}
    }
}

