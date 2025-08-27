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

        public static string GetUrlType(this string url)
        {

            if (url.Contains("list=RD"))
            {
                return "Mix";
            }

            try

            {
                var videoId = VideoId.Parse(url);
                return "Video";
            }

            catch(ArgumentException)
            {

            }

            try
            {
                var playlistId = PlaylistId.Parse(url);
                return "Playlist";
            }

            catch (ArgumentException)
            {

            }

            return "Link no valido";

        }

        public static string GetSafeFileName(this string title, string extension)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var safeName = string.Join("_", title.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            return safeName + extension;
        }
    }
}
