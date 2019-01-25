using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Updater {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private volatile bool _completed;
        public bool DownloadCompleted { get { return _completed; } }


        Dictionary<string, Uri> Files = new Dictionary<string, Uri>();
        


        public MainWindow()
        {
            InitializeComponent();
            AddFiles();
            StartDownload();
            
        }


        public void AddFiles()
        {
            Files.Add("Config.xml", new Uri(Settings.Default.URI + Settings.Default.Config));
            Files.Add("Film.m4v", new Uri("https://exitare.de/Film.m4v"));
        }


        private void StartDownload()
        {

            foreach (KeyValuePair<string, Uri> file in Files)
            {
                Console.WriteLine(file.Key + " " + file.Value);
                _completed = false;
                WebClient client = new WebClient();
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                client.DownloadFileAsync(file.Value, file.Key);




            }
           // progressBar.Maximum = bytes_total;
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
                       // Displays the operation identifier, and the transfer progress.
            Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);

            progressBar.Value = e.ProgressPercentage;
            StatusLabel.Content =  e.ProgressPercentage + " % complete... ( " + e.BytesReceived +  " / " + e.TotalBytesToReceive + ")";

        }


        private void DownloadFileCompleted(object sender , AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Download completed");
            StatusLabel.Content = "Download Finished";
            _completed = true;
        }


    }
}
