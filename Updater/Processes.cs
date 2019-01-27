using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater {
    class ProcessService {
        public static Dictionary<string, bool> ProcessDic = new Dictionary<string, bool>();


        public static void FillProcessData()
        {
            ProcessDic.Add("HIPA", false);
        }

        public static void CloseAllProcesses()
        {
            foreach (KeyValuePair<string, bool> ProcessData in ProcessService.ProcessDic)
            {
                foreach (Process proc in Process.GetProcesses())
                {
                    if (proc.ProcessName.Contains(ProcessData.Key))
                    {
                        Process[] myProcesses;
                        myProcesses = Process.GetProcessesByName(ProcessData.Key);
                        foreach (Process myProcess in myProcesses)
                        {
                            myProcess.Kill();
                        }
                    }
                }
            }
        }


        public static bool CheckHIPAOpened()
        {
            foreach(Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName.Contains("HIPA"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
