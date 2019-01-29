using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HIPA;

namespace HIPA.Services {
    class XML {
        public static Version LoadXML()
        {
            Version version = Version.Parse("0.0.0.0");
            try
            {
                XElement versions = XElement.Load(Settings.Default.ConfigFile);
                Console.WriteLine(versions);
                version = Version.Parse(versions.Element("Versions").Value);
                return version;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return version;
        }


    }
}
