﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using HIPA.Services.Updater;
using HIPA.Statics;


namespace HIPA
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            Debug.Print("Exeption occured");
            // Prevent default unhandled exception processing
            e.Handled = true;
        }

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
            if (UpdateHandler.CheckForUpdate() && Globals.ConnectionSuccessful)
                Globals.UpdateAvailable = true;

        
        }
    }
}
