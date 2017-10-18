using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExpiredPassportUpdater
{
    public class Settings
    {
        private static Object s_lock = new Object();
        private static Settings instance = null;

        private Settings()
        {
            //inicialize
            try
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DownloadFileURL"]))
                    throw new Exception("Initialize DownloadFileURL error");
                else
                    DownloadFileURL = ConfigurationManager.AppSettings["DownloadFileURL"];

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EndingDownloadFilePath"]))
                    throw new Exception("Initialize EndingDownloadFilePath error");
                else
                    EndingDownloadFilePath = ConfigurationManager.AppSettings["EndingDownloadFilePath"];

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DBSource"]))
                    throw new Exception("Initialize DBSource error");
                else
                    DBSource = ConfigurationManager.AppSettings["DBSource"];

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DBUserName"]))
                    throw new Exception("Initialize DBUserName error");
                else
                    DBUserName = ConfigurationManager.AppSettings["DBUserName"];

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DBPassword"]))
                    throw new Exception("Initialize DBPassword error");
                else
                    DBPassword = ConfigurationManager.AppSettings["DBPassword"];

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["TempTableName"]))
                    throw new Exception("Initialize TempTableName error");
                else
                    TempTableName = ConfigurationManager.AppSettings["TempTableName"];


                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DatabaseName"]))
                    throw new Exception("Initialize DatabaseName error");
                else
                    DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];


                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["InstanceName"]))
                    throw new Exception("Initialize InstanceName error");
                else
                    InstanceName = ConfigurationManager.AppSettings["InstanceName"];

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ServerUrl"]))
                    throw new Exception("Initialize ServerUrl error");
                else
                    ServerUrl = ConfigurationManager.AppSettings["ServerUrl"];


                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Key"]))
                    throw new Exception("Initialize Key error");
                else
                    Key = ConfigurationManager.AppSettings["Key"];

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EndingUnarchiveFilePath"]))
                    throw new Exception("Initialize EndingUnarchiveFilePath error");
                else
                    EndingUnarchiveFilePath = ConfigurationManager.AppSettings["EndingUnarchiveFilePath"];

                int countLoadetPerPageItems=0;
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CountLoadetPerPageItems"]) 
                    || !int.TryParse(ConfigurationManager.AppSettings["CountLoadetPerPageItems"], out countLoadetPerPageItems))
                    throw new Exception("Initialize CountPerPageLoadetItems error");
                else
                    CountLoadetPerPageItems = countLoadetPerPageItems;


                int countThreads = 0;
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CountThreads"])
                    || !int.TryParse(ConfigurationManager.AppSettings["CountThreads"], out countThreads))
                    throw new Exception("Initialize CountThreads error");
                else
                    CountThreads = countThreads;



                int countUpdateItemsPerRequest = 0;
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CountUpdateItemsPerRequest"])
                    || !int.TryParse(ConfigurationManager.AppSettings["CountUpdateItemsPerRequest"], out countUpdateItemsPerRequest))
                    throw new Exception("Initialize CountUpdateItemsPerRequest error");
                else
                    CountUpdateItemsPerRequest = countUpdateItemsPerRequest;


                

            }
            catch (Exception ex)
            {
                throw new SettingsException("Initialize Settings error", ex);
            }
        }
        public static Settings Instance
        {
            get
            {
                if (instance != null) return instance;
                lock (s_lock)
                {
                    Settings temp = new Settings();
                    instance = temp;
                }
                return instance;
            }
        }


        public string DownloadFileURL { get; private set; }
        public string EndingDownloadFilePath { get; private set; }
        public string EndingUnarchiveFilePath { get; private set; }
        public string TempTableName { get; private set; }
        public string DatabaseName { get; private set; }
        public string InstanceName { get; private set; }
        public string ServerUrl { get; private set; }
        public string Key { get; private set; }

        public string DBSource { get; private set; }
        public string DBUserName { get; private set; }
        public string DBPassword { get; private set; }
        public int CountLoadetPerPageItems { get; private set; }
        public int CountThreads { get; private set; }
        public int CountUpdateItemsPerRequest { get; private set; }
    }

    public class SettingsException : Exception
    {
        public SettingsException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public SettingsException(string message) : base(message)
        {
        }
    }
}
