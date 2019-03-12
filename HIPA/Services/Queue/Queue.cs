using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HIPA.Statics;

namespace HIPA.Queue {

    class Queue {


        public static void StartQueueProccessing()
        {
            Thread t = new Thread(QueueHandler);
            t.Start();
            Globals.LogQueue.Enqueue("1");
            Globals.LogQueue.Enqueue("2");
            Globals.LogQueue.Enqueue("3");
            Globals.LogQueue.Enqueue("4");
            Globals.LogQueue.Enqueue("5");
            Globals.LogQueue.Enqueue("6");
            Globals.LogQueue.Enqueue("7");
        }

        public static void QueueHandler()
        {
            while (true)
            {

                Debug.Print("Was executed");
                foreach (string message in Globals.LogQueue.ToList())
                {

                    Globals.LogQueue.Dequeue();
                }
                
            }

        }


        public static void AddLogMessageToQueue(string logmessage)
        {

        }
    }
}
