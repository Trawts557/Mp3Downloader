using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3DownloaderPro.Utils
{
    public static class YoutubeUtils
    {
        public static string GetCleanVideoUrl(string url)
        {
            var videoId = YoutubeExplode.Videos.VideoId.TryParse(url);
            if (videoId.HasValue)
            {
                return $"https://www.youtube.com/watch?v={videoId.Value}";
            }
            return url;
        }
    }
}
