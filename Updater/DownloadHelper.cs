using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater {
    class DownloadHelper {
        public static Dictionary<string, Uri> DownloadFiles = new Dictionary<string, Uri>();
        public static List<Uri> Liste = new List<Uri>();
        public static void AddDownloadFiles()
        {
            DownloadFiles.Add("Config.xml", new Uri(Settings.Default.URI + Settings.Default.Config));
            DownloadFiles.Add("HIPA.exe", new Uri(Settings.Default.URI + Settings.Default.HIPA));
            Liste.Add(new Uri(Settings.Default.URI + Settings.Default.Config));
            Liste.Add(new Uri(Settings.Default.URI + Settings.Default.HIPA));
        }
    }
}
