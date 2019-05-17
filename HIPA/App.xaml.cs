using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using HIPA.Services.Updater;
using HIPA.Statics;
using HIPA.Services.Log;


namespace HIPA
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message + "\n For more information have a look at the Logs folder", "HIPA", MessageBoxButton.OK, MessageBoxImage.Warning);
            Logger.logger.Error(e.Exception.StackTrace);
          
            e.Handled = true;
            Application.Current.Shutdown(21);
        }

        public enum ApplicationExitCode {
            Success = 0,
            Failure = 1,
            CantWriteToApplicationLog = 2,
            CantPersistApplicationState = 3
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
           // Logger.logger("Application closed", LogLevel.Info);
            Debug.Print("Closed");
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Logger.WriteLog("Application startup", LogLevel.Info);
            if (UpdateHandler.CheckForUpdate() && Globals.ConnectionSuccessful)
                Globals.UpdateAvailable = true;

        
        }
    }
}
