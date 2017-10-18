using ExpiredPassportUpdater.Model;
using Reputation.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Linq;

namespace ExpiredPassportUpdater.UtilityClasses
{
    class DBHelper
    {

        private static readonly Object s_lock = new Object();
        private static DBHelper instance = null;

        public static DBHelper Instance
        {
            get
            {
                if (instance != null) return instance;
                lock (s_lock)
                {
                    DBHelper temp = new DBHelper();
                    instance = temp;
                }
                return instance;
            }
        }
     
        string _databaseName;

        private DBHelper()
        {
            DataAccess.Config.DBSource = Settings.Instance.DBSource;
            DataAccess.Config.DBUserName = Settings.Instance.DBUserName;
            DataAccess.Config.DBPassword = Settings.Instance.DBPassword;
            DataAccess.Config.DataIsolationLevel = System.Data.IsolationLevel.ReadUncommitted;

          
            _databaseName = Settings.Instance.DatabaseName;

           
        }



        public long? IsExist(ExpiredPassports item)
        {
            try
            {
                using (var _dbHelper = new DatabaseHelper(false))
                {
                    var result = _dbHelper.Query<long>($@"SELECT [Id] FROM [{ _databaseName}].[dbo].[ExpiredPassports] WHERE [Serial]=@Serial AND [Number]=@Number", new { Serial = item.Serial, Number = item.Number });
                    if (result.Count() > 0)
                        return result.First();

                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        static Object AddLoker = new Object();
        public long AddItem(ExpiredPassports item)
        {
            try
            {
                lock (AddLoker)
                {
                    using (var _dbHelper = new DatabaseHelper(false))
                    {
                        return _dbHelper.Query<long>($@"INSERT INTO [{ _databaseName}].[dbo].[ExpiredPassports] ([Serial],[Number],[ExcludeDate]) VALUES(@Serial, @Number, null); SELECT CAST(SCOPE_IDENTITY() as bigint)", new { Serial = item.Serial, Number = item.Number }).Single();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<long> GetIdActualItems()
        {

            try
            {
                using (var _dbHelper = new DatabaseHelper(false))
                {
                    return _dbHelper.Query<long>($@"SELECT [Id] FROM [{ _databaseName}].[dbo].[ExpiredPassports] WHERE [ExcludeDate] IS NOT NULL", null);
                }
            }
            catch
            {
                throw;
            }
           
        }

        public void UpdateIrrelevantItems(IEnumerable<long> idsIrrelevantItems, DateTime excludeDate)
        {
            using (var _dbHelper = new DatabaseHelper(true))
            {
                try
                {

                    _dbHelper.Execute($@"UPDATE [{_databaseName}].[dbo].[ExpiredPassports]  SET  [ExcludeDate] = @excludeDate 
                                            WHERE [Id] IS @Ids", new { Ids = idsIrrelevantItems, excludeDate = excludeDate });

                    _dbHelper.Commit();

                }
                catch
                {
                    _dbHelper.Rollback();
                    throw;
                }
            }
            
        }

    }
}
