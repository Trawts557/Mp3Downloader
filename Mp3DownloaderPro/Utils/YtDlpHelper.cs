using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Common;

namespace Mp3DownloaderPro.Utils
{
    public static class YtDlpHelper
    {
        public static async Task DownloadVideoAsync(string url, string outputFolder, IProgress<int> progress)
        {
            var psi = new ProcessStartInfo
            {
                FileName = @"C:\yt-dlp\yt-dlp.exe",
                Arguments = $"--no-playlist -x --audio-format mp3 -o \"{outputFolder}\\%(title)s.%(ext)s\" {url}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = psi };
            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data) && e.Data.Contains("[download]"))
                {
                    // Regex mejorado para capturar diferentes formatos de porcentaje
                    var match = System.Text.RegularExpressions.Regex.Match(e.Data, @"(\d{1,3}(?:\.\d+)?)%");
                    if (match.Success)
                    {
                        int percent = (int)Math.Round(double.Parse(match.Groups[1].Value));
                        progress?.Report(percent);
                    }
                    // También capturamos el mensaje de descarga completada
                    else if (e.Data.Contains("100%") || e.Data.ToLower().Contains("downloaded"))
                    {
                        progress?.Report(100);
                    }
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            // Forzar 100% al finalizar por si acaso
            progress?.Report(100);
        }

        public static async Task DownloadPlaylistAsync(string url, string outputFolder, IProgress<int> progress)
        {
            var videos = await new YoutubeExplode.YoutubeClient().Playlists.GetVideosAsync(url).CollectAsync();
            int total = videos.Count;
            int current = 0;

            foreach (var video in videos)
            {
                // Creamos un progress handler para este video específico
                var videoProgress = new Progress<int>(percent =>
                {
                    // Calculamos la contribución de este video al progreso total
                    double videoWeight = 100.0 / total; // Peso de cada video
                    double completedVideosProgress = current * videoWeight; // Progreso de videos completados
                    double currentVideoProgress = (percent * videoWeight) / 100.0; // Progreso del video actual

                    // Progreso general combinado
                    int overall = (int)Math.Round(completedVideosProgress + currentVideoProgress);
                    
                    overall = Math.Min(100, overall);

                    progress?.Report(overall);
                });

                // Descargar el video actual
                await DownloadVideoAsync(video.Url, outputFolder, videoProgress);

                // Incrementar contador después de completar la descarga
                current++;
            }

            // Aseguramos que al final se reporte 100%
            progress?.Report(100);
        }
    }
}
