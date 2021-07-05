using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HLSSafariProxy_ASP.NET.Services.Interfaces;

namespace HLSSafariProxy_ASP.NET.Services
{
    public class SecondLevelManifestModifierService : ISecondLevelManifestModifierService
    {
        private readonly IFetcherService _fetcherService;
 
        private const string QUALITY_LEVEL_REGEX = @"(QualityLevels\(\d+\))";
        private const string FRAGMENTS_REGEX = @"(Fragments\([\w\d=-]+,[\w\d=-]+\))";
        private const string URL_REGEX = @"("")(https?:\/\/[\da-z\.-]+\.[a-z\.]{2,6}[\/\w \.-]*\/?[\?&][^&=]+=[^&=#]*)("")";

        public SecondLevelManifestModifierService(IFetcherService fetcherService)
        {
            _fetcherService = fetcherService;
        }
        
        public async Task<string> FetchAndModifyManifest(string playbackUrl, string token)
        {
            if (!token.Contains("Bearer"))
                token = "Bearer=" + token;

            string encodedToken = HttpUtility.UrlEncode(token);
 
            string baseUrl = playbackUrl.Substring(0, playbackUrl.IndexOf(".ism", System.StringComparison.OrdinalIgnoreCase)) + ".ism";
            string content = await _fetcherService.GetRawContent(playbackUrl);
 
            string newContent = Regex.Replace(content, URL_REGEX, string.Format(CultureInfo.InvariantCulture, "$1$2&token={0}$3", encodedToken));
            Match match = Regex.Match(playbackUrl, QUALITY_LEVEL_REGEX);
            if (match.Success)
            {
                var qualityLevel = match.Groups[0].Value;
                newContent = Regex.Replace(newContent, FRAGMENTS_REGEX, m => string.Format(CultureInfo.InvariantCulture, baseUrl + "/" + qualityLevel + "/" + m.Value));
            }

            return newContent;
        }
    }
}