using ExpiredPassportUpdater.Model;
using ExpiredPassportUpdater.UtilityClasses;
using ExpiredPassportUpdater.FileUtility;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ExpiredPassportUpdater
{

    class Program
    {
        public static Logger _logger;

        static Object AddArrayLoker = new Object(); 
        static Object PageLoker = new Object();  

        static void Main(string[] args)
        {
           
            try
            {
                _logger = LogManager.GetCurrentClassLogger();
                var loadDate = DateTime.Now;
                Console.WriteLine("Обновление данных: Start {0}", loadDate.ToString());

         
               FileUpdater.Instance.UpdateFileInFolder();
                string pathToUpdatedFile = FileUpdater.Instance.GetPathToUpdatedFile();

 
                string pathToUnarchiveFile = Environment.CurrentDirectory + Settings.Instance.EndingUnarchiveFilePath;
                FileHelper.DeleteFileIfExist(pathToUnarchiveFile);
                FileHelper.BZipUnarchiveFromNewFile(pathToUpdatedFile, pathToUnarchiveFile);


                int countPassportsDataInFile = FileHelper.GetCountLinesFromCVSFile(pathToUnarchiveFile);

                long[] idsActualItems = new long[countPassportsDataInFile-1];//первый элемент просто текст- пропускаем
                int lastAddIndex = 0;




                int currentPage = 0;
                int countPage = (int)Math.Ceiling((double)countPassportsDataInFile / (double)Settings.Instance.CountLoadetPerPageItems);
                int skip = 0;

                Task[] tasks = new Task[Settings.Instance.CountThreads];
                for (int i = 0; i < Settings.Instance.CountThreads; i++)
                    tasks[i] = Task.Factory.StartNew(new Action(() =>
                    {

                        while (true)
                        {
                            lock (PageLoker)
                            {
                                if (currentPage >= countPage)
                                    break;
                                
                                skip = currentPage * Settings.Instance.CountLoadetPerPageItems + 1;//первый элемент просто текст- пропускаем
                                currentPage++;
                            }
                            string[] unzippedLines = FileReader.ReadCVSFileInParts(pathToUnarchiveFile, skip, Settings.Instance.CountLoadetPerPageItems);

                            var actualItems = Parser.ToExpiredPassports(unzippedLines);

                            foreach (var item in actualItems)
                            {
                                long? id = DBHelper.Instance.IsExist(item);
                                if (id == null)
                                    id = DBHelper.Instance.AddItem(item);

                                lock (AddArrayLoker)
                                {

                                    if (lastAddIndex % 100000 == 0)
                                        Console.WriteLine($"Обработанно: {lastAddIndex}");


                                    idsActualItems[lastAddIndex] = (long)id;
                                    lastAddIndex++;
                                }
                            } 
                        }
                    }));
                Task.WaitAll(tasks);







                long[] idsIrrelevantItems = DBHelper.Instance.GetIdActualItems().ToArray().Where(i => !idsActualItems.Any(id => id == i)).ToArray();



                int count = (int)Math.Ceiling((double)idsIrrelevantItems.Length / (double)Settings.Instance.CountUpdateItemsPerRequest);
                for (int i=0; i<count; i++)
                DBHelper.Instance.UpdateIrrelevantItems(idsIrrelevantItems.Skip(i* Settings.Instance.CountUpdateItemsPerRequest).Take(Settings.Instance.CountUpdateItemsPerRequest), loadDate);


                FileHelper.DeleteFileIfExist(pathToUnarchiveFile);

                Statistics.Instance.SendStatistics();
                Statistics.Instance.SendMessage("Данные обновлены.");

                Console.WriteLine("Обновление данных завершено: End {0}", DateTime.Now.ToString());
                Console.WriteLine("Затраченно времени: {0}", DateTime.Now- loadDate);
            }
            catch(SettingsException ex)
            {
                _logger.Fatal(ex.ToString() + Environment.NewLine);
                Console.WriteLine("Error! При инициализации ностроек. Проверить App.config!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex.ToString()+Environment.NewLine);
                Statistics.Instance.SendMessage("Error! При обновлении данных произошла ошибка.");

                Console.WriteLine("Error! При обновлении данных произошла ошибка.");
            }

           Console.ReadLine();
        }
    }
}
