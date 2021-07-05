using System.Net;
using System.Text;
using System.Threading.Tasks;
using HLSSafariProxy_ASP.NET.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HLSSafariProxy_ASP.NET.Controllers
{
    [Route("[controller]")]
    public class AzureMediaServicesProxyController : Controller
    {
        private readonly IUrlService _urlService;
        private readonly ITopManifestModifierService _topManifestModifier;
        private readonly ISecondLevelManifestModifierService _secondLevelManifestModifierService;

        public AzureMediaServicesProxyController(IUrlService urlService,
            ITopManifestModifierService topManifestModifier,
            ISecondLevelManifestModifierService secondLevelManifestModifierService)
        {
            _urlService = urlService;
            _topManifestModifier = topManifestModifier;
            _secondLevelManifestModifierService = secondLevelManifestModifierService;
        }
        
        /// <summary>
        /// Creates overwritten top-level manifest
        /// </summary>
        /// <param name="playbackUrl">URL to the video hosted within Azure Media Serivces ending with /manifest</param>
        /// <param name="token">Authorization token for the video</param>
        /// <returns>Returns modified top-level manifest</returns>
        [HttpGet]
        [Route("[action]")]
        [Produces("application/vnd.apple.mpegurl")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTopLevelPlaylist([FromQuery] string playbackUrl, [FromQuery] string token)
        {
            if (playbackUrl.ToLowerInvariant().EndsWith("manifest"))
                playbackUrl += "(format=m3u8-aapl)";

            if (!playbackUrl.Contains("format=m3u8-aapl"))
                return BadRequest("Playback URL doesn't contain the format specification");

            var secondLevelManifestUrl = _urlService
                .GetSecondLevelManifestProxyUrl(Request, "AzureMediaServicesProxy/GetSecondLevelPlaylist");

            var modifiedTopLevelManifest = await _topManifestModifier
                .FetchAndModifyManifest(playbackUrl, token, secondLevelManifestUrl);

            return Content(modifiedTopLevelManifest, "application/vnd.apple.mpegurl", Encoding.UTF8);
        }
        
        /// <summary>
        /// Creates overwritten second-level manifest
        /// </summary>
        /// <param name="playbackUrl">URL to the video hosted within Azure Media Serivces ending with /manifest</param>
        /// <param name="token">Authorization token for the video</param>
        /// <returns>Returns modified second-level manifest</returns>
        [HttpGet]
        [Route("[action]")]
        [Produces("application/vnd.apple.mpegurl")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSecondLevelPlaylist([FromQuery] string playbackUrl, [FromQuery] string token)
        {
            var modifiedSecondLevelManifest = await _secondLevelManifestModifierService
                .FetchAndModifyManifest(playbackUrl, token);

            return Content(modifiedSecondLevelManifest, "application/vnd.apple.mpegurl", Encoding.UTF8);
        }
    }
}