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

          
                // Create a new WebClient instance.
                using (WebClient webClient = new WebClient())
                {
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
                if(newVersion > actualVersion)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
          
        }


        public static void StartUpdates()
        {
            
        }

    }
}
