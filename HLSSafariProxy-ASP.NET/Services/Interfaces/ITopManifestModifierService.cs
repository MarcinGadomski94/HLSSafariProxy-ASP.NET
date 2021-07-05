using System.Threading.Tasks;

namespace HLSSafariProxy_ASP.NET.Services.Interfaces
{
    public interface ITopManifestModifierService
    {
        /// <summary>
        /// Fetchers and modifies top-level manifest for video playback
        /// </summary>
        /// <param name="playbackUrl">URL of the Video hosted within Azure Media Services ending with /manifest</param>
        /// <param name="token">Authorization token for video</param>
        /// <param name="secondLevelManifestUrl">Proxy URL of the GET endpoint for fetching second-level manifest</param>
        /// <returns>Returns modified top-level manifest</returns>
        Task<string> FetchAndModifyManifest(string playbackUrl, string token, string secondLevelManifestUrl);
    }
}