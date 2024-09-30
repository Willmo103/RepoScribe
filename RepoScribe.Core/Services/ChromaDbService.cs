using System;
using System.Net.Http;
using System.Threading.Tasks;
using RepoScribe.Core.ContentItems;
using Newtonsoft.Json;

namespace RepoScribe.Core.Services
{
    public class ChromaDbService
    {
        private static readonly Lazy<ChromaDbService> _instance = new Lazy<ChromaDbService>(() => new ChromaDbService());
        public static ChromaDbService Instance => _instance.Value;

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        private ChromaDbService()
        {
            _httpClient = new HttpClient();
            _baseUrl = "http://localhost:8000"; // TODO: Update with your ChromaDB URL
        }

        public async Task UpsertAsync(ContentItem contentItem)
        {
            // Convert ContentItem to ChromaDB format
            var requestData = new
            {
                id = contentItem.Id,
                content = contentItem.Content,
                metadata = new
                {
                    contentItem.Path,
                    contentItem.Owner,
                    contentItem.LastModified,
                    contentItem.SizeMB,
                    contentItem.Language,
                    Domain = contentItem.Domain.ToString(),
                    ContextSource = contentItem.ContextSource.ToString()
                }
            };

            var jsonContent = JsonConvert.SerializeObject(requestData);
            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/upsert", httpContent);
            response.EnsureSuccessStatusCode();
        }

        // Additional methods for querying, deleting, etc.
        // TODO: abstract the metadata into a class specific metadata objects to be used in the ChromaDB service
        // TODO: add methods for querying and deleting content items from ChromaDB
    }
}
