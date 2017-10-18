using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExpiredPassportUpdater.FileUtility
{

    class FileUpdater
    {
        private static FileUpdater instance = null;

        private FileUpdater()
        {
            _filePath = Environment.CurrentDirectory + Settings.Instance.EndingDownloadFilePath;
            _downloadFileURL = Settings.Instance.DownloadFileURL;
        }
        public static FileUpdater Instance
        {
            get
            {
                if (instance != null) return instance;
                FileUpdater temp = new FileUpdater();
                instance = temp;
                return instance;
            }
        }

        static string _filePath;
        static string _downloadFileURL;

        public string PathToLoadFile { get; set; }

        public string GetPathToUpdatedFile()
        {
            return _filePath;
        }


        public void UpdateFileInFolder()
        {
            try
            {
                if (File.Exists(_filePath))
                File.Delete(_filePath);
           
            DownloadFile();
            }
            catch (Exception ex)
            {
                throw new Exception("Update file from folder error", ex);
            }
        }






        private void DownloadFile()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(_downloadFileURL, _filePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File downloading error", ex);
            }
        }
    }
}
