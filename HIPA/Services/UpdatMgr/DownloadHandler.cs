using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HIPA.Statics;
using HIPA.Services.Log;
using System.Diagnostics;
using System.IO;
using HIPA.Services.Misc;
using System.Runtime.InteropServices;

namespace HIPA.Services.Updater
{

    class DownloadHandler
    {

        public static void DownloadSetup()
        {
            // Create a new WebClient instance.
            string exeURL = Settings.Default.URL + Settings.Default.SetupEXE;
            string msiURL = Settings.Default.URL + Settings.Default.SetupMSI ;

            using (WebClient webClient = new WebClient())
            {
                string url = Settings.Default.URL + Settings.Default.SetupEXE;
                // Download the Web resource and save it into the current filesystem folder.
                try
                {

                    WebProxy wp = new WebProxy(Settings.Default.Proxy_URL, Settings.Default.Proxy_Port);
                    if (Settings.Default.Proxy_Active)
                        webClient.Proxy = wp;

                    webClient.DownloadFile(exeURL, Globals.HIPATempFolderSetupEXEFileName);
                    webClient.DownloadFile(msiURL, Globals.HIPATempFolderSetupMSIFileName);
                }
                catch (Exception ex)
                {
                    Logger.logger.Warn("There was an error downloading the Updater!");
                    Logger.logger.Warn(ex.Message);
                    Logger.logger.Warn(ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        Logger.logger.Warn(ex.InnerException.Message);
                        Logger.logger.Warn(ex.InnerException.StackTrace);
                    }
                }

            }
        }
    }


}


