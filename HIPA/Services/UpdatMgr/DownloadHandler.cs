using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HIPA.Statics;
using HIPA.Services.Log;
using System.Diagnostics;

namespace HIPA.Services.Updater
{

    class DownloadHandler
    {

        public delegate void EventHandler(EventArgs args);
        public static event EventHandler OnDownloadError = delegate { };


        public static void DownloadFiles()
        {
            DownloadUpdater();
            DownloadConfig();
        }


        public static void DownloadUpdater()
        {
            string remoteUri = Settings.Default.URL;
            string fileName = Settings.Default.Updater;

            // Create a new WebClient instance.
            using (WebClient webClient = new WebClient())
            {
                string url = remoteUri + fileName;
                // Download the Web resource and save it into the current filesystem folder.
                try
                {
                   
                    WebProxy wp = new WebProxy(Settings.Default.Proxy_URL, Settings.Default.Proxy_Port);
                    if (Settings.Default.Proxy_Active)
                        webClient.Proxy = wp;

                    webClient.DownloadFile(url, fileName);
                    Globals.ConnectionSuccessful = true;
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine("Could not download Updater");
                    Debug.WriteLine(ex.Message);
#endif
                    Logger.WriteLog("There was an error downloading the Updater!", LogLevel.Warning);
                    Logger.WriteLog(ex.Message, LogLevel.Warning);

                    EventHandler handler = OnDownloadError;
                    DownloadErrorArgs args = new DownloadErrorArgs();
                    args.Message = ex.Message;
                    args.TimeReached = DateTime.Now;
                    OnDownloadError(args);

                    Globals.ConnectionSuccessful = false;
                }

            }
        }

        public static void DownloadConfig()
        {
            string remoteUri = Settings.Default.URL;
            string fileName = Settings.Default.ConfigFile;


            using (WebClient webClient = new WebClient())
            {
                try
                {
                    WebProxy wp = new WebProxy(Settings.Default.Proxy_URL, Settings.Default.Proxy_Port);
                    if (Settings.Default.Proxy_Active)
                        webClient.Proxy = wp;

                    webClient.DownloadFile(remoteUri + fileName, fileName);
                    Globals.ConnectionSuccessful = true;
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex.Message, LogLevel.Error);
                    Globals.ConnectionSuccessful = false;
                }
            }
        }


     
    }


    public class DownloadErrorArgs : EventArgs {
        public string Message { get; set; }
        public DateTime TimeReached { get; set; }
    }
}


