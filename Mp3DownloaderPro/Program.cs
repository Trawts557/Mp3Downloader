namespace Mp3DownloaderPro
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary> https://www.youtube.com/watch?v=Ik0TPM9g5dI&list=PLA5VzP2LmrYex9Fq1rFTak6siBS3lfcQe&index=1
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}