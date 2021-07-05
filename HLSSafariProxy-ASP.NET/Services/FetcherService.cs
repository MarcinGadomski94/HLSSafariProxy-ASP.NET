using System.Net.Http;
using System.Threading.Tasks;
using HLSSafariProxy_ASP.NET.Services.Interfaces;

namespace HLSSafariProxy_ASP.NET.Services
{
    public class FetcherService : IFetcherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        public FetcherService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetRawContent(string url)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}