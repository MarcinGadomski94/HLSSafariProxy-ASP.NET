using System.Threading.Tasks;

namespace HLSSafariProxy_ASP.NET.Services.Interfaces
{
    public interface IFetcherService
    {
        /// <summary>
        /// Fetches raw string content from the provided URL
        /// </summary>
        /// <param name="url">URL to fetch raw string from</param>
        /// <returns>Returns fetched raw string</returns>
        Task<string> GetRawContent(string url);
    }
}