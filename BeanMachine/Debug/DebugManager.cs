using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace BeanMachine.Debug
{
    public static class DebugManager
    {
        private static Dictionary<string, object> _trackedObjects = new Dictionary<string, object>();

        private static List<object> _logs = new List<object>();
        private static List<string> _logsTime = new List<string>();

        public static bool GameQuit = false;

        public static void Monitor(object obj, string name)
        {
            foreach (KeyValuePair<string, object> i in _trackedObjects)
            {
                if (i.Key == name)
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

        public static void SendDataToDebugConsole()
        {
            try
            {
                IPAddress ipaddress = IPAddress.Parse("127.0.0.1");

                TcpListener mylist = new TcpListener(ipaddress, 8000);
                mylist.Start();

                Console.WriteLine("Server is Running on Port: 8000");
                Console.WriteLine("Local endpoint:" + mylist.LocalEndpoint);

                Console.WriteLine("Waiting for Connections...");
                Socket socket = mylist.AcceptSocket();

                Console.WriteLine("Connection Accepted From:" + socket.RemoteEndPoint);

                while (true)
                {
                    try
                    {
                        byte[] bytes = new byte[100000];
                        int k = socket.Receive(bytes);

                        Console.WriteLine("Recieved..");

                        string request = "";
                        string responce = "";

                        for (int i = 0; i < k; i++)
                        {
                            request += Convert.ToChar(bytes[i]);
                        }

                        Console.WriteLine(request);

                        if (request == "DebugInfo")
                        {
                            responce = ParseDebugInfoToJson();
                        }

                        if (request == string.Empty)
                        {
                            throw new Exception("No Request");
                        }

                        ASCIIEncoding asencd = new ASCIIEncoding();
                        socket.Send(asencd.GetBytes(responce));


                    }
                    catch (Exception)
                    {
                        mylist.Stop();
                        socket.Close();

                        SendDataToDebugConsole();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error.." + ex.StackTrace);
            }

        }

        private static string ParseDebugInfoToJson()
        {
            DebugFile file = new DebugFile();

            file.LoggedValues = new List<string>();
            file.LoggedValuesTimes = new List<string>();

            file.MonitoredValues = new List<string>();
            file.MonitoredValuesNames = new List<string>();

            foreach (KeyValuePair<string, object> pair in _trackedObjects)
            {
                file.MonitoredValues.Add(pair.Value.ToString());
                file.MonitoredValuesNames.Add(pair.Key.ToString());
            }

            if (_logs.Count > 1000)
            {
                _logs.RemoveAt(0);
                _logsTime.RemoveAt(0);
            }

            foreach (object obj in _logs.ToArray())
            {

                file.LoggedValues.Add(obj.ToString());
                _logs.Remove(obj);
            }

            foreach (string obj in _logsTime.ToArray())
            {
                file.LoggedValuesTimes.Add(obj.ToString());
                _logsTime.Remove(obj);
            }

            Console.WriteLine(JsonConvert.SerializeObject(file));
            return JsonConvert.SerializeObject(file);
        }
    }
}
