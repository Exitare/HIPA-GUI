using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HIPA;
using XML;
namespace Update {
    public class Update {
        public static bool CheckForUpdates(Version actualVersion)
        {
            bool[] update = new bool[2];

            Console.WriteLine("Check for Updates");
            string remoteUri = Settings.Default.URL;
            string fileName = Settings.Default.ConfigFile;
            WebClient webClient = new WebClient();
            WebProxy wp = new WebProxy(Settings.Default.Proxy_URL, Settings.Default.Proxy_Port);

            if (Settings.Default.Proxy_Active)
                webClient.Proxy = wp;
            
            try
            {
                string downloadLink = remoteUri + fileName;
                webClient.DownloadFile(downloadLink, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured");
                Console.WriteLine(ex.Message);

            }

            Version newVersion = XML.XML.LoadXML();
            return newVersion > actualVersion ?  true : false;
        }


        public static void StartUpdates()
        {
            Process.Start("Updater.exe");
        }

    }
}
