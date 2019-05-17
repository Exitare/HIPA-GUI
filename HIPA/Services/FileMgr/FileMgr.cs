using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPA.Statics;

namespace HIPA.Services.FileMgr {

    class FileMgr
    {

        public static void CreateFiles()
        {
            CreateLogFiles();
        }

        private static void CreateLogFiles()
        {
            if (!File.Exists(Globals.LogFileName))
                File.Create(Globals.LogFileName).Close();


            if (!File.Exists(Globals.ErrorLogFileName))
                File.Create(Globals.ErrorLogFileName).Close();

        }

        public static bool CheckCustomPath()
        {
            if (Settings.Default.CustomOutputPathActive)
            {
                if (Settings.Default.CustomOutputPath == "")
                {
                    Settings.Default.CustomOutputPathActive = false;
                    Settings.Default.Save();
                    return false;
                }


                try
                {
                    // Attempt to get a list of security permissions from the folder. 
                    // This will raise an exception if the path is read only or do not have access to view the permissions. 
                    System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(Settings.Default.CustomOutputPath);
                    return true;
                }
                catch (UnauthorizedAccessException)
                {
                    Settings.Default.CustomOutputPathActive = false;
                    Settings.Default.Save();
                    return false;
                }
            }

            return true;

        }

        public static bool CheckCustomPath(string path)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(path);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                Settings.Default.CustomOutputPathActive = false;
                Settings.Default.Save();
                return false;
            }
        }

    }
}
