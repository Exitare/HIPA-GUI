using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HIPA;
using HIPA.Statics;
using HIPA.Services.XML;
using System.Runtime.InteropServices;
using HIPA.Services.Misc;
using HIPA.Services.Log;
using static System.Net.Mime.MediaTypeNames;

namespace HIPA.Services.Updater {

    public class UpdateHandler  {

        [DllImport("wininet.dll")]
        public extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool IsConnectedToInternet()
        {
            try
            {

                int Desc;
                if(InternetGetConnectedState(out Desc, 0))
                {
                    Logger.logger.Info("Internet connection available!");
                    Globals.ConnectionSuccessful = true;
                    return true;
                }
                Logger.logger.Info("Internet connection not available!");
                Globals.ConnectionSuccessful = false;
                return false;

            }
            catch
            {
                Logger.logger.Info("Internet connection not available!");
                Globals.ConnectionSuccessful = false;
                return false;
            }
        }


        public static void CheckForUpdate()
        {
            if (IsConnectedToInternet())
            {
                DownloadHandler.DownloadSetup();
                if(GetRemoteVersion() > Version.Parse(FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion))
                    Globals.UpdateAvailable = true;
            }
        
        }



        public static Version GetRemoteVersion()
        {
            Version ver = new Version(0, 0, 0, 0);
            string urlAddress = "https://raw.githubusercontent.com/hipa-org/HIPA-GUI/dev/HIPA/Properties/AssemblyInfo.cs";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }


                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();

                Version.TryParse(MiscHandler.GetStringBetween(data, "AssemblyFileVersion(\"", "\")"), out ver);

            }
            return ver;
        }


        public static void StartUpdate()
        {
            Process.Start(Globals.HIPATempFolderSetupEXEFileName);
            System.Windows.Application.Current.Shutdown();
        }

    }
}
