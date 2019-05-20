using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HIPA;
using HIPA.Services.Log;

namespace HIPA.Services.XML {

    partial class XMLHandler {

        public static Version LoadConfigVersion()
        {
            Version version = Version.Parse("0.0.0.0");
            try
            {
                return Version.Parse(XElement.Load(Settings.Default.ConfigFile).Element("Versions").Value);
            }
            catch (Exception ex)
            {
                //Logger.WriteLog("Could not load XML File", LogLevel.Error);
                //Logger.WriteLog(ex.Message, LogLevel.Error);
            }

            return version;
        }


    }
}
