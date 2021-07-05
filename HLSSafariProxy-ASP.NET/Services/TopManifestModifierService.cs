using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HLSSafariProxy_ASP.NET.Services.Interfaces;

namespace HLSSafariProxy_ASP.NET.Services
{
    public class TopManifestModifierService : ITopManifestModifierService
    {
        private readonly IFetcherService _fetcherService;
        
        private const string QUALITY_LEVEL_REGEX = @"(QualityLevels\(\d+\)/Manifest\(.+\))";

        public TopManifestModifierService(IFetcherService fetcherService)
        {
            _fetcherService = fetcherService;
        }
        
        public async Task<string> FetchAndModifyManifest(string playbackUrl, string token, string secondLevelManifestUrl)
        {
            if (!token.Contains("Bearer"))
                token = "Bearer=" + token;
 
            string topLevelManifestContent = await _fetcherService.GetRawContent(playbackUrl);
 
            string topLevelManifestBaseUrl = playbackUrl.Substring(0, playbackUrl.IndexOf(".ism", System.StringComparison.OrdinalIgnoreCase)) + ".ism";
            string urlEncodedTopLeveLManifestBaseUrl = HttpUtility.UrlEncode(topLevelManifestBaseUrl);
            string urlEncodedToken = HttpUtility.UrlEncode(token);
            
            MatchEvaluator encodingReplacer = m => $"{secondLevelManifestUrl}?playbackUrl={urlEncodedTopLeveLManifestBaseUrl}{HttpUtility.UrlEncode("/" + m.Value)}&token={urlEncodedToken}";
 
            string newContent = Regex.Replace(topLevelManifestContent, QUALITY_LEVEL_REGEX, encodingReplacer);
 
            return newContent;
        }
    }
}