using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum SettingsOptionsEnum { Calculations, Network, General };

namespace HIPA.Statics {


    partial class SettingsOptions {
        private static List<string> settingsMenuPoints = new List<string> { SettingsOptionsEnum.Calculations.ToString(), SettingsOptionsEnum.Network.ToString(), SettingsOptionsEnum.General.ToString() };

        public static List<string> SettingsMenuPoints { get => settingsMenuPoints; set => settingsMenuPoints = value; }
    }
}
