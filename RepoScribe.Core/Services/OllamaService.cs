using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoScribe.Core.Services
{
    internal class OllamaService
    {
        private HttpClient _httpClient;
        private string _baseUrl;


        public OllamaService()
        {
            _httpClient = new HttpClient();
            _baseUrl = "http://localhost/api/v1/";
        }

        public async Task<string> GetAsync(string url, Dictionary<string, string> req)
        {
            var response = await _httpClient.GetAsync(_baseUrl + url + req);
            return await response.Content.ReadAsStringAsync();
        }

    }
}