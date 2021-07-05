using Microsoft.AspNetCore.Http;

namespace HLSSafariProxy_ASP.NET.Services.Interfaces
{
    public interface IUrlService
    {
        /// <summary>
        /// Generates proxy second-level manifest URL
        /// </summary>
        /// <param name="httpRequest">Request object that comes from the controller</param>
        /// <param name="secondLevelManifestEndpoint">Path for GET proxy endpoint for fetching second-level manifest</param>
        /// <returns>Returns proxy URL of second level manifest</returns>
        string GetSecondLevelManifestProxyUrl(HttpRequest httpRequest, string secondLevelManifestEndpoint);
    }
}