using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExpiredPassportUpdater.UtilityClasses
{
    class Statistics
    {
        private static Object s_lock = new Object();
        private static Statistics instance = null;

        public static Statistics Instance
        {
            get
            {
                if (instance != null) return instance;
                Statistics temp = new Statistics();
                instance = temp;
                return instance;
            }
        }

        string _serverUrl;
        string _key;
        string _instanceName;


        public int InsertedCount { get; set; }
        public int UpdatedCount { get; set; }

        public Statistics()
        {
            _serverUrl = Settings.Instance.ServerUrl;
            _key = Settings.Instance.Key;
            _instanceName = Settings.Instance.InstanceName;
        }

        public void SendStatistics()
        {
            new StatisticSender(_serverUrl, _key).SendSnapshot(_instanceName, new Dictionary<string, int> { { "InsertedCount", InsertedCount }, { "UpdatedCount", UpdatedCount } });
        }

        public void SendMessage(string message)
        {
            new StatisticSender(_serverUrl, _key).SendMessage(_instanceName, message);
        }
    }

    class StatisticSender
    {
        public StatisticSender(string serverUrl, string key)
        {
            _serverUrl = serverUrl;
            _key = key;
        }


        private string _serverUrl;
        private string _key;

       
        public void SendSnapshot(string instanceName, Dictionary<string, int> measurments)
        {
            string url = string.Format("{0}/Client/SendStatistics?key={1}&instanceName={2}&measurments=", _serverUrl, _key, instanceName);

            foreach (var item in measurments)
            {
                url += item.Key + "~" + item.Value + "*";
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            var response = request.GetResponse();
            response.Dispose();
        }
        public void SendMessage(string instanceName, string msg)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_serverUrl + "/Client/SendMessage?key=" + _key + "&instanceName=" + instanceName + "&message=" + msg);

            var response = request.GetResponse();
            response.Dispose();
        }
    }
}
