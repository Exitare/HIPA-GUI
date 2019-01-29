using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HIPA.Statics;

namespace HIPA.Services
{
    class Download
    {
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
                    Console.WriteLine("Could not download Updater");
                    Console.WriteLine(ex.Message);
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

                    //if (Settings.Default.Proxy_Active)
                    //   webClient.Proxy = wp;
                    webClient.DownloadFile(remoteUri + fileName, fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured");
                    Console.WriteLine(Settings.Default.URL);
                    Console.WriteLine(ex.Message);

                }
            }
        }
    }
}



////public RequestClass(String proxyURL, int port, String username, String password)
//{
//    //Validate proxy address
//    var proxyURI = new Uri(string.Format("{0}:{1}", proxyURL, port));

////Set credentials
//ICredentials credentials = new NetworkCredential(username, password);

//    //Set proxy
//    this.proxy =  = new WebProxy(proxyURI, true, null, credentials);
//}