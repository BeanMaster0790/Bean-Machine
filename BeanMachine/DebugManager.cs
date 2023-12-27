using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProtectTheCenter.Managers
{
    public static class DebugManager
    {
        private static Dictionary<string, object> _trackedObjects= new Dictionary<string, object>();
        private static List<object> _logs = new List<object>();
        private static List<string> _logsTime = new List<string>();

        public static void Monitor(object obj, string name)
        {
            foreach(KeyValuePair<string, object> i in _trackedObjects)
            {
                if(i.Key == name)
                {
                    _trackedObjects.Remove(i.Key);
                }
            }

            _trackedObjects.Add(name, obj);
        }

        public static void Log(object obj)
        {
            _logs.Add(obj);
            _logsTime.Add(string.Format("{0} : {1} : {2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
        }

        public static void Update()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("------------Logs------------");

                if(_logs.Count > 15)
                {
                    while (_logsTime.Count > 15)
                    {
                        _logs.RemoveAt(0);
                        _logsTime.RemoveAt(0);
                    }
                }

                object[] logsClone = _logs.ToArray();
                object[] logsTimeClone = _logsTime.ToArray();

                for (int i = 0; i < logsClone.Length; i++)
                {
                    Console.WriteLine(string.Format("{0} | {1},", logsTimeClone[i], logsClone[i]));
                }

                Console.WriteLine("------------Tracked Values------------");

                Dictionary<string, object> clonedDictonary = _trackedObjects.ToDictionary(entry => entry.Key, entry => entry.Value); 

                foreach(KeyValuePair<string, object> i in clonedDictonary)
                {
                    Console.WriteLine($"{i.Key} : {i.Value}");
                }

                Console.WriteLine("\n\n\n");

                Thread.Sleep(100);
            }

        }
    }
}
