using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace HIPA
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public enum ApplicationExitCode {
            Success = 0,
            Failure = 1,
            CantWriteToApplicationLog = 2,
            CantPersistApplicationState = 3
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
           
            Debug.Print("Closed");
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Set window location
            if (Settings.Default.WindowLocation != null)
            {
                this.Location = Settings.Default.WindowLocation;
            }

            // Set window size
            if (Settings.Default.WindowSize != null)
            {
                this.Size = Settings.Default.WindowSize;
            }

            string remoteUri = Settings.Default.URL;
            string fileName = Settings.Default.Updater;

            // Create a new WebClient instance.
            using (WebClient webClient = new WebClient())
            {
               string url = remoteUri + fileName;
                // Download the Web resource and save it into the current filesystem folder.
                try
                {
                    webClient.DownloadFile(url, fileName);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Could not download Updater");
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}
