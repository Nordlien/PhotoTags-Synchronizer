using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using SqliteDatabase;
using NLog;
using NLog.Fluent;

namespace GoogleLocationHistory
{
    public class GoogleLocationHistoryJson
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public delegate void MyEvent(object sender, long locationsCount, long filePosition, long fileLength);
        public event EventHandler LocationFound;
        public event MyEvent LocationFoundParam;

        private SqliteDatabaseUtilities dbTools;
        public GoogleLocationHistoryJson(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }

        #region ReadJsonAndWriteToCache
        public GoogleLocationHistoryItems ReadJsonAndWriteToCache(String jsonPath, String userName, bool returnGoogleLocationHistoryItems)
        {
        
            GoogleLocationHistoryDatabaseCache googleLocationDatabaseCache = new GoogleLocationHistoryDatabaseCache(dbTools);
            googleLocationDatabaseCache.WriteLocationHistorySource(userName, jsonPath);
            
            GoogleLocationHistoryItems googleLocationHistory = null;
            if (returnGoogleLocationHistoryItems)
            {
                googleLocationHistory = new GoogleLocationHistoryItems(userName);
            }
                

            int locationsFoundCount = 0;
            var dinamic = new DinamicStreamJsonParser()
            {
                PropertyToType = new Dictionary<string, Type>()
                {
                    { "locations", Type.GetType(Type.GetType("GoogleLocationHistory.GoogleJsonLocations").AssemblyQualifiedName)},
                    { "activity", Type.GetType(Type.GetType("GoogleLocationHistory.GoogleJsonActivity").AssemblyQualifiedName)},
                },
                ObjectFoundCallback = (object obj, long readPosition, long fileLength) => 
                {                   
                    if (obj.GetType() == typeof(GoogleJsonLocations))
                    {
                        locationsFoundCount++;
                        if (returnGoogleLocationHistoryItems) googleLocationHistory.Add ((GoogleJsonLocations)obj);
                        
                        googleLocationDatabaseCache.WriteLocationHistory(userName, (GoogleJsonLocations)obj);

                        LocationFound?.Invoke(this, EventArgs.Empty);
                        LocationFoundParam?.Invoke(this, locationsFoundCount, readPosition, fileLength);
                    }

                },
                NewTypeFoundCallback = (object obj, string name) => {
                    Logger.Debug($"Found object {obj} with name {name}");
                },
                StreamReader = new StreamReader(jsonPath, System.Text.Encoding.UTF8, true, 1000),
                
            };
            dinamic.Parse();

            return googleLocationHistory;

        }
        #endregion 
    }


}

