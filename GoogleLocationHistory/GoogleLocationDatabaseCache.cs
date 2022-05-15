#define MonoSqlite
#define noMicrosoftDataSqlite

#if MonoSqlite
using Mono.Data.Sqlite;
#elif MicrosoftDataSqlite
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif



using System;
using System.IO;
using MetadataLibrary;
using SqliteDatabase;
using System.Collections.Generic;
using LocationNames;

namespace GoogleLocationHistory
{
    public class GoogleLocationHistoryDatabaseCache
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region GoogleLocationHistoryDatabaseCache
        private SqliteDatabaseUtilities dbTools;
        public GoogleLocationHistoryDatabaseCache(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }
        #endregion

        #region TransactionBeginSelect
        public SqliteTransaction TransactionBegin()
        {
            return dbTools.TransactionBegin();
        }
        #endregion

        #region TransactionCommitSelect
        public void TransactionCommit(SqliteTransaction sqliteTransaction)
        {
            dbTools.TransactionCommit(sqliteTransaction);
        }
        #endregion

        #region WriteLocationHistorySource
        public void WriteLocationHistorySource(string userAccount, string fileNamePath)
        {
            if (File.Exists(fileNamePath))
            {
                Mono.Data.Sqlite.SqliteTransaction sqlTransaction;
                do
                {
                    sqlTransaction = dbTools.TransactionBegin();

                    #region INSERT INTO LocationSource 
                    string sqlCommand =
                        "INSERT INTO LocationSource (UserAccount, FileDirectory, FileName, FileDateModified, FileDateImported) " +
                        "Values (@UserAccount, @FileDirectory, @FileName, @FileDateModified, @FileDateImported)";
                    using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
                    {
                        //commandDatabase.Prepare();
                        commandDatabase.Parameters.AddWithValue("@UserAccount", userAccount);
                        commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fileNamePath));
                        commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fileNamePath));
                        commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(File.GetLastWriteTimeUtc(fileNamePath)));
                        commandDatabase.Parameters.AddWithValue("@FileDateImported", dbTools.ConvertFromDateTimeToDBVal(DateTime.UtcNow));
                        commandDatabase.ExecuteNonQuery();      // Execute the query
                    }
                    #endregion
                    
                    dbTools.TransactionCommit(sqlTransaction);
                } while (!dbTools.TransactionCommit(sqlTransaction)) ;
            }
        }
        #endregion

        #region WriteLocationHistory
        public void WriteLocationHistory(string userAccount, GoogleJsonLocations googleJsonLocations)
        {
            #region INSERT OR IGNORE INTO LocationHistory
            string sqlCommand =
                "INSERT OR IGNORE INTO LocationHistory (UserAccount, TimeStamp, Latitude, Longitude, Altitude, Accuracy) " +
                "Values (@UserAccount, @TimeStamp, @Latitude, @Longitude, @Altitude, @Accuracy) ";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase)) //, sqlTransactionSelect))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@UserAccount", userAccount);
                commandDatabase.Parameters.AddWithValue("@TimeStamp", dbTools.ConvertFromDateTimeToDBVal(googleJsonLocations.Timestamp));
                commandDatabase.Parameters.AddWithValue("@Latitude", googleJsonLocations.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", googleJsonLocations.Longitude);
                commandDatabase.Parameters.AddWithValue("@Altitude", googleJsonLocations.Altitude);
                commandDatabase.Parameters.AddWithValue("@Accuracy", googleJsonLocations.Accuracy);
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            #endregion
        }
        #endregion

        #region FindBestLocationBasedOtherMediaFiles
        public Metadata FindBestLocationBasedOtherMediaFiles(DateTime? locationDateTime, DateTime? mediaDateTaken, DateTime? fileDate, int acceptDiffrentSecound)
        {
            List<Metadata> metadatas = FindLocationBasedOtherMediaFiles(locationDateTime, mediaDateTaken, fileDate, acceptDiffrentSecound);
            if (metadatas.Count < 1)
                return null;
            else 
                return metadatas[0];
        }
        #endregion

        #region FindLocationBasedOtherMediaFiles
        public List<Metadata> FindLocationBasedOtherMediaFiles(DateTime? locationDateTime, DateTime? mediaDateTaken, DateTime? fileDate, int acceptDiffrentSecound)
        {
            if (locationDateTime == null && mediaDateTaken == null && fileDate == null) return null;

            string sqlCommand = "";

            #region LocationDateTime
            if (locationDateTime != null)
                sqlCommand +=
                "(SELECT 1 AS Priority, LocationDateTime AS Date, ABS(LocationDateTime - @LocationDateTime) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE LocationDateTime >= @LocationDateTime " +
                "AND  LocationLatitude != 0 AND LocationLongitude != 0 " +
                "AND (LocationLatitude IS NOT NULL OR LocationLongitude IS NOT NULL) " +
                "ORDER BY LocationDateTime LIMIT 3) " +
                "UNION SELECT * FROM " +
                "(SELECT 1 AS Priority, LocationDateTime AS Date, ABS(LocationDateTime - @LocationDateTime) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE LocationDateTime <= @LocationDateTime " +
                "AND  LocationLatitude != 0 AND LocationLongitude != 0 " +
                "AND (LocationLatitude IS NOT NULL OR LocationLongitude IS NOT NULL) " +
                "ORDER BY LocationDateTime DESC LIMIT 3) ";
            #endregion

            #region MediaDateTaken
            if (mediaDateTaken != null) sqlCommand += 
                (sqlCommand == "" ? "" : "UNION SELECT * FROM ") +
                "(SELECT 2 AS Priority, MediaDateTaken AS Date, ABS(MediaDateTaken - @MediaDateTaken) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE MediaDateTaken >= @MediaDateTaken " +
                "AND  LocationLatitude != 0 AND LocationLongitude != 0 " +
                "AND (LocationLatitude IS NOT NULL OR LocationLongitude IS NOT NULL) " +
                "ORDER BY MediaDateTaken LIMIT 3) " +
                "UNION SELECT * FROM " +
                "(SELECT 2 AS Priority, MediaDateTaken AS Date, ABS(MediaDateTaken - @MediaDateTaken) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE MediaDateTaken <= @MediaDateTaken " +
                "AND  LocationLatitude != 0 AND LocationLongitude != 0 " +
                "AND (LocationLatitude IS NOT NULL OR LocationLongitude IS NOT NULL) " +
                "ORDER BY MediaDateTaken DESC LIMIT 3) ";
            #endregion

            #region FileDateCreated
            if (fileDate != null) sqlCommand +=
                (sqlCommand == "" ? "" : "UNION SELECT * FROM ") +
                "(SELECT 3 AS Priority, FileDateCreated AS Date, ABS(FileDateCreated - @FileDateCreated) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE FileDateCreated >= @FileDateCreated " +
                "AND  LocationLatitude != 0 AND LocationLongitude != 0 " +
                "AND (LocationLatitude IS NOT NULL OR LocationLongitude IS NOT NULL) " +
                "ORDER BY FileDateCreated LIMIT 3) " +
                "UNION SELECT * FROM " +
                "(SELECT 3 AS Priority, FileDateCreated AS Date, ABS(FileDateCreated - @FileDateCreated) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE FileDateCreated <= @FileDateCreated " +
                "AND  LocationLatitude != 0 AND LocationLongitude != 0 " +
                "AND (LocationLatitude IS NOT NULL OR LocationLongitude IS NOT NULL) " +
                "ORDER BY FileDateCreated DESC LIMIT 3) ";
            #endregion

            List<Metadata> metadatas;
            Mono.Data.Sqlite.SqliteTransaction sqlTransactionSelect;
            do
            {
                sqlTransactionSelect = dbTools.TransactionBeginSelect();

                #region SELECT FROM MediaMetadata
                sqlCommand = "SELECT * FROM " + sqlCommand +
                "ORDER BY Priority, TimeDistance ";
                //"LIMIT 1";

                metadatas = new List<Metadata>();
                using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
                {
                    //commandDatabase.Prepare();
                    if (locationDateTime != null) commandDatabase.Parameters.AddWithValue("@LocationDateTime", dbTools.ConvertFromDateTimeToDBVal(locationDateTime));
                    if (mediaDateTaken != null) commandDatabase.Parameters.AddWithValue("@MediaDateTaken", dbTools.ConvertFromDateTimeToDBVal(mediaDateTaken));
                    if (fileDate != null) commandDatabase.Parameters.AddWithValue("@FileDateCreated", dbTools.ConvertFromDateTimeToDBVal(fileDate));
                    //if (fileDateCreated != null) commandDatabase.Parameters.AddWithValue("@FileDateCreated", dbTools.ConvertFromDateTimeToDBVal(fileDateCreated));

                    using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            long? priority = dbTools.ConvertFromDBValLong(reader["Priority"]);
                            DateTime? date = dbTools.ConvertFromDBValDateTimeLocal(reader["Date"]);
                            long? timeDistance = dbTools.ConvertFromDBValLong(reader["TimeDistance"]);
                            float? locationLatitude = dbTools.ConvertFromDBValFloat(reader["LocationLatitude"]);
                            float? locationLongitude = dbTools.ConvertFromDBValFloat(reader["LocationLongitude"]);

                            if (timeDistance / 100000 < acceptDiffrentSecound)
                            {
                                Metadata metadata = new Metadata(MetadataBrokerType.GoogleLocationHistory);
                                switch (priority)
                                {
                                    case 3:
                                        metadata.FileDateCreated = date;
                                        break;
                                    case 2:
                                        metadata.MediaDateTaken = date;
                                        break;
                                    case 1:
                                        metadata.LocationDateTime = date;
                                        break;
                                }
                                metadata.LocationLatitude = locationLatitude;
                                metadata.LocationLongitude = locationLongitude;
                                bool doesCoordinatesExist = false;
                                foreach (Metadata metadataToCheck in metadatas)
                                {
                                    if (metadataToCheck.LocationLatitude == metadata.LocationLatitude &&
                                        metadataToCheck.LocationLongitude == metadata.LocationLongitude) doesCoordinatesExist = true;
                                }
                                if (!doesCoordinatesExist) metadatas.Add(metadata);
                            }
                        }

                    }
                }
                #endregion

            } while (!dbTools.TransactionCommitSelect(sqlTransactionSelect));

            return metadatas;
        }
        #endregion

        #region LoadLocationHistory
        public HashSet<LocationsHistory> LoadLocationHistory(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            HashSet<LocationsHistory> locationsHistories = new HashSet<LocationsHistory>();

            Mono.Data.Sqlite.SqliteTransaction sqlTransactionSelect;
            do
            {
                sqlTransactionSelect = dbTools.TransactionBeginSelect();

                #region SELECT UserAccount, TimeStamp, Latitude, Longitude, Altitude, Accuracy FROM LocationHistory
                string sqlCommand = "SELECT UserAccount, TimeStamp, Latitude, Longitude, Altitude, Accuracy FROM LocationHistory WHERE " +
                "TimeStamp >= @dateTimeFrom AND TimeStamp <= @dateTimeTo " +
                "ORDER BY TimeStamp LIMIT 5000";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@dateTimeFrom", dbTools.ConvertFromDateTimeToDBVal(dateTimeFrom));
                commandDatabase.Parameters.AddWithValue("@dateTimeTo", dbTools.ConvertFromDateTimeToDBVal(dateTimeTo));
                //commandDatabase.Parameters.AddWithValue("@UserAccount", userAccount);

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LocationsHistory locationsHistory = new LocationsHistory();
                        locationsHistory.UserAccount = dbTools.ConvertFromDBValString(reader["UserAccount"]);
                        locationsHistory.Timestamp = (DateTime)dbTools.ConvertFromDBValDateTimeUtc(reader["TimeStamp"]);
                        locationsHistory.LocationCoordinate = new LocationCoordinate(
                            (float)dbTools.ConvertFromDBValFloat(reader["Latitude"]), (float)dbTools.ConvertFromDBValFloat(reader["Longitude"])
                            );
                        locationsHistory.Altitude = (float)dbTools.ConvertFromDBValFloat(reader["Altitude"]);
                        locationsHistory.Accuracy = (float)dbTools.ConvertFromDBValFloat(reader["Accuracy"]);
                        locationsHistories.Add(locationsHistory);
                    }
                }
            }
                #endregion

            } while (!dbTools.TransactionCommitSelect(sqlTransactionSelect));

            return locationsHistories;
        }
        #endregion

        #region FindLocationBasedOnTime
        public Metadata FindLocationBasedOnTime(String userAccount, DateTime? datetime, int acceptDiffrentSecound)
        {
            Metadata metadataResult = null;
            Mono.Data.Sqlite.SqliteTransaction sqlTransactionSelect;
            do
            {
                sqlTransactionSelect = dbTools.TransactionBeginSelect();

                #region SELECT UserAccount, TimeStamp, Latitude, Longitude, Altitude, Accuracy FROM LocationHistory
                //I could use pythagoras to get excact distance, but I don't see the point of doing that
                string sqlCommand = 
                "SELECT UserAccount, TimeStamp, Latitude, Longitude, Altitude, Accuracy FROM LocationHistory WHERE " +
                "UserAccount = @UserAccount AND " +
                "( " +
                "TimeStamp = (SELECT MAX(TimeStamp) AS TimeStampPrevious FROM LocationHistory WHERE " +
                "TimeStamp <= @TimeStamp " +
                "AND UserAccount = @UserAccount) " +
                "OR " +
                "TimeStamp = " +
                "(SELECT MIN(TimeStamp) AS TimeStampNext FROM LocationHistory WHERE " +
                "TimeStamp >= @TimeStamp " +
                "AND UserAccount = @UserAccount) " +
                ")";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@TimeStamp", dbTools.ConvertFromDateTimeToDBVal(datetime));
                commandDatabase.Parameters.AddWithValue("@UserAccount", userAccount);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    Metadata metadata = new Metadata(MetadataBrokerType.GoogleLocationHistory);
                    DateTime? minTimeStamp = null;
                    float? minLatitude = null;
                    float? minLongitude = null;
                    float? minAltitude = null;
                    float? minAccuracy = null;

                    if (reader.Read())
                    {
                        #region Read minLatitude, minLongitude, minAltitude
                        minTimeStamp = (DateTime)dbTools.ConvertFromDBValDateTimeUtc(reader["TimeStamp"]); 
                        minLatitude = dbTools.ConvertFromDBValFloat(reader["Latitude"]);
                        minLongitude = dbTools.ConvertFromDBValFloat(reader["Longitude"]);
                        minAltitude = dbTools.ConvertFromDBValFloat(reader["Altitude"]);
                        minAccuracy = dbTools.ConvertFromDBValFloat(reader["Accuracy"]);
                        
                        if (minTimeStamp == datetime)
                        {
                            metadata.LocationLatitude = minLatitude;
                            metadata.LocationLongitude = minLongitude;
                            metadata.LocationAltitude = minAltitude;
                        }
                        #endregion 

                        if (reader.Read())
                        {
                            #region Read maxLatitude, maxLongitude, maxAltitude, maxAccuracy
                            DateTime? maxTimeStamp = (DateTime)dbTools.ConvertFromDBValDateTimeUtc(reader["TimeStamp"]);
                            float? maxLatitude = dbTools.ConvertFromDBValFloat(reader["Latitude"]);
                            float? maxLongitude = dbTools.ConvertFromDBValFloat(reader["Longitude"]);
                            float? maxAltitude = dbTools.ConvertFromDBValFloat(reader["Altitude"]);
                            float? maxAccuracy = dbTools.ConvertFromDBValFloat(reader["Accuracy"]);
                            #endregion

                            #region Calc diff
                            Double lowDiffInSeconds = Math.Abs((minTimeStamp - datetime).Value.TotalSeconds);
                            Double highDiffInSeconds = Math.Abs((maxTimeStamp - datetime).Value.TotalSeconds);
                            Double totalDifInSeconds = lowDiffInSeconds + highDiffInSeconds;
                            #endregion

                            #region If diff < acceptDiffrentSecound - Set result metadata
                            //if (Math.Min(lowDiffInSeconds, highDiffInSeconds) < acceptDiffrentSecound)
                            if (totalDifInSeconds < acceptDiffrentSecound ||
                                lowDiffInSeconds < acceptDiffrentSecound * 0.01 ||
                                highDiffInSeconds < acceptDiffrentSecound * 0.01)
                            {
                                metadata.LocationAltitude = (float)
                                        (((minAltitude * (totalDifInSeconds - lowDiffInSeconds)) +
                                        (maxAltitude * (totalDifInSeconds - highDiffInSeconds)))
                                        / totalDifInSeconds);
                                metadata.LocationLatitude = (float)
                                        (((minLatitude * (totalDifInSeconds - lowDiffInSeconds)) +
                                        (maxLatitude * (totalDifInSeconds - highDiffInSeconds)))
                                        / totalDifInSeconds);
                                metadata.LocationLongitude = (float)
                                        (((minLongitude * (totalDifInSeconds - lowDiffInSeconds)) +
                                        (maxLongitude * (totalDifInSeconds - highDiffInSeconds)))
                                        / totalDifInSeconds);

                                metadataResult = metadata;
                            }
                            #endregion
                        }
                    }
                }
            }
                #endregion

            } while (!dbTools.TransactionCommitSelect(sqlTransactionSelect));

            return metadataResult;
        }
        #endregion
    }
}

