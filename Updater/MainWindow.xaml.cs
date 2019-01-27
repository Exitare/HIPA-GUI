using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private volatile bool _download_completed;
        public bool DownloadCompleted { get { return _download_completed; } }


        public MainWindow()
        {

            InitializeComponent();
            ProcessService.FillProcessData();
            ProcessService.CloseAllProcesses();
            DownloadHelper.AddDownloadFiles();
            StartDownload();
            //Process.Start("HIPA");
        }


        private async void StartDownload()
        {
            foreach(KeyValuePair<string, Uri> file in DownloadHelper.DownloadFiles)
            {
                await Downloadfile(file.Key, file.Value);
              
            }

            MessageBox.Show("Update complete.\nPress Ok to restart HIPA");
            Process.Start("HIPA");

            if (ProcessService.CheckHIPAOpened())
            {
                Application.Current.Shutdown();
            }
        }


        private async Task<bool> DownloadHttpFile(string filename, Uri URI)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    
                    HttpResponseMessage response = await client.GetAsync(URI);
                    int length = int.Parse(response.Content.Headers.First(h => h.Key.Equals("Content-Length")).Value.First());
                    Debug.Print(length.ToString());
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Above three lines can be replaced with new helper method below
                    // string responseBody = await client.GetStringAsync(uri);

                    //Console.WriteLine(responseBody);
                    return true;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    return false;
                }
            }
        }

        private async Task<bool> Downloadfile(string filename, Uri uri)
        {
            try
            {
                Console.WriteLine(filename + " " + uri);
                _download_completed = false;
                WebClient client = new WebClient();
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadfilecompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadprogresscallback);
                await client.DownloadFileTaskAsync(uri, filename);
                return true;
            } catch(HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        
        }

        private void downloadprogresscallback(object sender, DownloadProgressChangedEventArgs e)
        {
            // displays the operation identifier, and the transfer progress.
            //  console.writeline("{0}    downloaded {1} of {2} bytes. {3} % complete...",
            //(string)e.userstate,
            //e.bytesreceived,
            //e.totalbytestoreceive,
            //e.progresspercentage);
            this.Dispatcher.Invoke(() =>
            {

                progressBar.Value = e.ProgressPercentage;
                StatusLabel.Content = e.ProgressPercentage + " % complete... ( " + e.BytesReceived + " / " + e.TotalBytesToReceive + ")";
            });




        }


        private void downloadfilecompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("download completed");
            this.Dispatcher.Invoke(() =>
            {
                StatusLabel.Content = "download finished";
            });

            _download_completed = true;

        }


    }
}
