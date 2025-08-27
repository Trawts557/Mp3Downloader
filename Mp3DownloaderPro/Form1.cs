using AngleSharp.Text;
using Mp3DownloaderPro.Utils;
using YoutubeExplode;
using YoutubeExplode.Common;
using System.Linq;

namespace Mp3DownloaderPro
{
    public partial class Form1 : Form
    {
        private readonly YoutubeClient _youtubeClient;
        private List<string> lPlaylist = new List<string>(); 


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
            listView1.View = View.Details;
            listView1.Columns.Add("Video", 600); // ancho de la columna
            listView1.Columns.Add("Estado", 200);

        }
        private void btnFolderPath_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputFolder.Text = fbd.SelectedPath;
                }
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;

            try
            {

                var url = LinkTxt.Text;

                if (string.IsNullOrWhiteSpace(txtOutputFolder.Text))
                {
                    MessageBox.Show("Debe insertar una ubicacion para realizar la descarga");
                    return;
                }



                var outputFolder = txtOutputFolder.Text;

                var progress = new Progress<int>(percent =>
                {
                    int safePercent = Math.Max(0, Math.Min(100, percent));
                    progressBar1.Value = safePercent;
                });

                if (url.GetUrlType() == "Video")
                {
                    var video = await _youtubeClient.Videos.GetAsync(url);

                    await YtDlpHelper.DownloadVideoAsync(url, outputFolder, progress);

                }
                else if (url.GetUrlType() == "Playlist")
                {
                    // CREAR UNA COPIA DE LA LISTA para iterar de forma segura
                    var videosToDownload = new List<string>(lPlaylist);

                    foreach (var videoUrl in videosToDownload)
                    {
                        var cleanedUrl = videoUrl.GetCleanVideoUrl();
                        await YtDlpHelper.DownloadVideoAsync(cleanedUrl, outputFolder, progress);
                    }

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
            var url = LinkTxt.Text;
            

            try
            {
                listView1.Items.Clear();

                if (url.GetUrlType() == "Link no valido")
                {
                    MessageBox.Show("Debe insertar un enlace");
                }

                if (url.GetUrlType() == "Video")
                {
                    var video = await _youtubeClient.Videos.GetAsync(url);

                    AddVideoToList(video.Title, video.Url);
                    btnDownload.Enabled = true;
                }

                else if (url.GetUrlType() == "Playlist")
                {
                    var playlist = await _youtubeClient.Playlists.GetVideosAsync(url).CollectAsync();

                    foreach (var video in playlist)
                    {
                        AddVideoToList(video.Title, video.Url);
                        
                        lPlaylist.Add(video.Url);
                    }
                    btnDownload.Enabled = true;
                }

                else if (url.GetUrlType() == "Mix")
                {

                    byte amountOfVideosToDownload = 26;
                    var playlist = await _youtubeClient.Playlists.GetVideosAsync(url).Take(amountOfVideosToDownload);
                                      
                    foreach (var video in playlist)
                    {
                        AddVideoToList(video.Title, video.Url);

                        lPlaylist.Add(video.Url);

                    }
                    
                   
                    btnDownload.Enabled = true;
                }

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

                var url = item.Tag as string;
                if (url == null) return;

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
        private void AddVideoToList(string title, string url)
        {
            var item = new ListViewItem(title);
            item.Tag = url;

            item.SubItems.Add("Activo"); // Estado inicial
            listView1.Items.Add(item);
        }
    }
}   

