using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using HIPA.Downloads;
namespace HIPA
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {
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
            Download.DownloadUpdater();
          
        }
    }
}
