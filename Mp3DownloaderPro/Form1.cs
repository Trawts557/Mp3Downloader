using Mp3DownloaderPro.Utils;
using YoutubeExplode;
using YoutubeExplode.Common;

namespace Mp3DownloaderPro
{
    public partial class Form1 : Form
    {
        private readonly YoutubeClient _youtubeClient;
        private List<string> lPlaylist = [];
        private string Url => LinkTxt.Text;
        private string OutputFolder => txtOutputFolder.Text;
        private ImageList imageList;

        public Form1()
        {
            InitializeComponent();
            _youtubeClient = new YoutubeClient();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            imageList = new ImageList();
            imageList.ImageSize = new Size(140, 90);

            listView1.View = View.Details;
            listView1.SmallImageList = imageList;

            listView1.Columns.Add("Video", 700);
            listView1.Columns.Add("Estado", -2, HorizontalAlignment.Left);


        }

        private async Task<Image> DownloadImageAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(url);
                using (var ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
        }

        private void btnFolderPath_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = fbd.SelectedPath;
            }

        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;

            try
            {
                var progress = new Progress<int>(percent =>
                {
                    int safePercent = Math.Max(0, Math.Min(100, percent));
                    progressBar1.Value = safePercent;
                });

                if (string.IsNullOrWhiteSpace(txtOutputFolder.Text))
                {
                    throw new Exception("Debe insertar una ubicacion para realizar la descarga");
                }

                switch (Url.GetUrlType())
                {
                    case UrlType.Video:

                        var video = await _youtubeClient.Videos.GetAsync(Url);

                        await YtDlpHelper.DownloadVideoAsync(Url, OutputFolder, progress);
                        break;

                    case UrlType.Playlist:

                        // CREA UNA COPIA DE LA LISTA para iterar de forma segura
                        var videosToDownload = new List<string>(lPlaylist);

                        foreach (var videoUrl in videosToDownload)
                        {
                            var cleanedUrl = videoUrl.GetCleanVideoUrl();
                            await YtDlpHelper.DownloadVideoAsync(cleanedUrl, OutputFolder, progress);
                        }
                        break;
                }

                MessageBox.Show("Descarga completada con éxito!");

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            finally
            {
                btnDownload.Enabled = true;
                progressBar1.Value = 0;
            }

        }

        private async void btnGetVideos_Click(object sender, EventArgs e)
        {
            //Video https://www.youtube.com/watch?v=V3pmJHRZB1Y
            //Playlist https://www.youtube.com/playlist?list=PLA5VzP2LmrYdYqyDxjx72P_O45rwm2VTx

            btnGetVideos.Enabled = false;
            btnDownload.Enabled = false;

            try
            {
                listView1.Items.Clear();

                switch (Url.GetUrlType())
                {

                    case UrlType.Invalid:
                        {
                            if (Url.GetUrlType() == UrlType.Invalid)
                            {
                                throw new Exception("Debe insertar un enlace");
                            }
                            break;
                        }

                    case UrlType.Video:
                        {
                            var video = await _youtubeClient.Videos.GetAsync(Url);

                            var thumbnailUrl = video.Thumbnails.GetWithHighestResolution().Url;

                            await AddVideoToList(video.Title, video.Url, video.Id, thumbnailUrl);

                            btnDownload.Enabled = true;

                            break;
                        }


                    case UrlType.Playlist:
                        {
                            var playlist = await _youtubeClient.Playlists.GetVideosAsync(Url).CollectAsync();

                            foreach (var video in playlist)
                            {
                                var thumbnailUrl = video.Thumbnails.GetWithHighestResolution().Url;

                                await AddVideoToList(video.Title, video.Url, video.Id, thumbnailUrl);

                                lPlaylist.Add(video.Url);
                            }

                            break;
                        }

                    case UrlType.Mix:
                        {
                            byte amountOfVideosToDownload = 26;
                            var playlist = await _youtubeClient.Playlists.GetVideosAsync(Url).Take(amountOfVideosToDownload);

                            foreach (var video in playlist)
                            {
                                var thumbnailUrl = video.Thumbnails.GetWithHighestResolution().Url;

                                await AddVideoToList(video.Title, video.Url, video.Id, thumbnailUrl);

                                lPlaylist.Add(video.Url);

                            }
                        }

                        break;
                }

                btnDownload.Enabled = true;

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            finally
            {
                btnGetVideos.Enabled = true;

            }

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems[0];

            if (listView1.SelectedItems.Count > 0)
            {
                /*
                var url = item.Tag as string;
                if (url == null) return;
                */

                if (item.Tag is not string url) return;

                if (item.Font.Strikeout)
                {
                    item.Font = new Font(item.Font, FontStyle.Regular);
                    item.ForeColor = Color.Black;
                    item.SubItems[1].Text = "Activo";

                    lPlaylist.Add(url);
                }

                else
                {
                    item.Font = new Font(item.Font, FontStyle.Strikeout);
                    item.ForeColor = Color.Gray;
                    item.SubItems[1].Text = "Inactivo";

                    lPlaylist.Remove(url);

                }
            }

        }

        private async Task AddVideoToList(string title, string url, string videoId, string thumbnailUrl)
        {
            var image = await DownloadImageAsync(thumbnailUrl);

            if (!imageList.Images.ContainsKey(videoId))
            {
                imageList.Images.Add(videoId, image);
            }

            var item = new ListViewItem(title);
            item.Tag = url;

            item.ImageKey = videoId;
            item.SubItems.Add("Activo");
            listView1.Items.Add(item);

        }

        private void listView1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    if (item.Tag is string url)
                    {

                        if (item.Font.Strikeout)
                        {
                            item.Font = new Font(item.Font, FontStyle.Regular);
                            item.ForeColor = Color.Black;
                            item.SubItems[1].Text = "Activo";

                            lPlaylist.Add(url);
                        }

                        else
                        {
                            item.Font = new Font(item.Font, FontStyle.Strikeout);
                            item.ForeColor = Color.Gray;
                            item.SubItems[1].Text = "Inactivo";

                            lPlaylist.Remove(url);

                        }

                    }


                }
            }
        }
    }
}   

