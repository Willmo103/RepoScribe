using System.Net.Http;
using System.Threading.Tasks;

namespace RepoScribe.Core.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAsync(string url)
        {
            // Implement HTTP GET request
            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string url, HttpContent content)
        {
            // Implement HTTP POST request
            var response = await _httpClient.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        // Add other HTTP methods as needed
    }
}
