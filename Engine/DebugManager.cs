using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static void StopMonitoring(object obj)
        {
            foreach(KeyValuePair<string, object> i in _trackedObjects)
            {
                if(i.Value == obj)               
                _trackedObjects.Remove(i.Key);
            }
        }

        public static void Log(object obj)
        {
            _logs.Add(obj);
            _logsTime.Add(string.Format("{0} : {1} : {2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
        }

        public static void Update()
        {
            Console.Clear();

            Console.WriteLine("------------Logs------------");

            if(_logs.Count > 15)
            {
                _logs.RemoveAt(0);
                _logsTime.RemoveAt(0);
            }

            for (int i = 0; i < _logs.Count; i++)
            {
                Console.WriteLine(string.Format("{0} | {1},", _logsTime[i], _logs[i]));
            }

            Console.WriteLine("------------Tracked Values------------");

            foreach(KeyValuePair<string, object> i in _trackedObjects)
            {
                Console.WriteLine($"{i.Key} : {i.Value}");
            }

            Console.WriteLine("\n\n\n");

        }
    }
}
