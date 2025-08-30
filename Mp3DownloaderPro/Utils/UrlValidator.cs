using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Playlists;
using YoutubeExplode.Videos;

namespace Mp3DownloaderPro.Utils
{
    public static class UrlValidator
    {
        public static string GetCleanVideoUrl(this string url)
        {
            var videoId = VideoId.TryParse(url);
            if (videoId.HasValue)
            {
                return $"https://www.youtube.com/watch?v={videoId.Value}";
            }
            return url;
        }

        public static UrlType GetUrlType(this string url)
        {

            if (url.Contains("list=RD", StringComparison.OrdinalIgnoreCase))
            {
                return UrlType.Mix;
            }

            else if (url.Contains("playlist?list=", StringComparison.OrdinalIgnoreCase))
            {
                return UrlType.Playlist;
            }

            else if (url.Contains("watch?v=", StringComparison.OrdinalIgnoreCase))
            {
                return UrlType.Video;
            }

            return UrlType.Invalid;

        }
    }
}
