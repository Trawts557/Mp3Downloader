namespace Mp3DownloaderPro
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            LinkTxt = new TextBox();
            label2 = new Label();
            btnFolderPath = new Button();
            btnDownload = new Button();
            progressBar1 = new ProgressBar();
            txtOutputFolder = new TextBox();
            btnGetVideos = new Button();
            listView1 = new ListView();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 43);
            label1.Name = "label1";
            label1.Size = new Size(118, 15);
            label1.TabIndex = 0;
            label1.Text = "Insert the playlist link";
            label1.Click += label1_Click;
            // 
            // LinkTxt
            // 
            LinkTxt.Location = new Point(150, 40);
            LinkTxt.Name = "LinkTxt";
            LinkTxt.Size = new Size(618, 23);
            LinkTxt.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 9);
            label2.Name = "label2";
            label2.Size = new Size(144, 15);
            label2.TabIndex = 2;
            label2.Text = "Playlist Video Downloader";
            label2.Click += label2_Click;
            // 
            // btnFolderPath
            // 
            btnFolderPath.Location = new Point(33, 74);
            btnFolderPath.Name = "btnFolderPath";
            btnFolderPath.Size = new Size(111, 25);
            btnFolderPath.TabIndex = 3;
            btnFolderPath.Text = "Folder Path";
            btnFolderPath.UseVisualStyleBackColor = true;
            btnFolderPath.Click += btnFolderPath_Click;
            // 
            // btnDownload
            // 
            btnDownload.Enabled = false;
            btnDownload.Location = new Point(61, 467);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(114, 27);
            btnDownload.TabIndex = 4;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(181, 432);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(586, 62);
            progressBar1.TabIndex = 5;
            // 
            // txtOutputFolder
            // 
            txtOutputFolder.Location = new Point(150, 76);
            txtOutputFolder.Name = "txtOutputFolder";
            txtOutputFolder.Size = new Size(617, 23);
            txtOutputFolder.TabIndex = 7;
            // 
            // btnGetVideos
            // 
            btnGetVideos.Location = new Point(61, 432);
            btnGetVideos.Name = "btnGetVideos";
            btnGetVideos.Size = new Size(114, 29);
            btnGetVideos.TabIndex = 8;
            btnGetVideos.Text = "Get videos";
            btnGetVideos.UseVisualStyleBackColor = true;
            btnGetVideos.Click += btnGetVideos_Click;
            // 
            // listView1
            // 
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Location = new Point(32, 111);
            listView1.Name = "listView1";
            listView1.Size = new Size(735, 315);
            listView1.TabIndex = 9;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.DoubleClick += listView1_DoubleClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(808, 506);
            Controls.Add(listView1);
            Controls.Add(btnGetVideos);
            Controls.Add(txtOutputFolder);
            Controls.Add(progressBar1);
            Controls.Add(btnDownload);
            Controls.Add(btnFolderPath);
            Controls.Add(label2);
            Controls.Add(LinkTxt);
            Controls.Add(label1);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "Mp3 Youtube Downloader";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox LinkTxt;
        private Label label2;
        private Button btnFolderPath;
        private Button btnDownload;
        private ProgressBar progressBar1;
        private TextBox txtOutputFolder;
        private Button btnGetVideos;
        private ListView listView1;
    }
}
