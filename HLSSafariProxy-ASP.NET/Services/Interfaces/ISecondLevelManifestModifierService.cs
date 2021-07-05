using System.Threading.Tasks;

namespace HLSSafariProxy_ASP.NET.Services.Interfaces
{
    public interface ISecondLevelManifestModifierService
    {
        /// <summary>
        /// Fetches and overwrites the second-level manifest
        /// </summary>
        /// <param name="playbackUrl">Video URL hosted within Azure Media Services ending with /manifest</param>
        /// <param name="token">Authorization token for the video</param>
        /// <returns>Returns the modified second-level manifest</returns>
        Task<string> FetchAndModifyManifest(string playbackUrl, string token);
    }
}