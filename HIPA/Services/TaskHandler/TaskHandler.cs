using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA.Services.TaskHandler
{

    partial class TaskHandler
    {
        static SortedDictionary<string, Task> TaskList = new SortedDictionary<string, Task>();

        public static void AddTask(string name, Task task)
        {
            TaskList.Add(name, task);
        }

        public static void RemoveTask(string name)
        {
            TaskList.Remove(name);
        }

        public static Task GetTask(string name)
        {
            foreach(KeyValuePair<string, Task> Pair in TaskList)
            {
                if (Pair.Key == name)
                    return Pair.Value;
            }

            return null;
        }
    }
}
