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

namespace GoogleLocationHistory
{
    public class GoogleLocationHistoryDatabaseCache
    {
        private SqliteDatabaseUtilities dbTools;
        public GoogleLocationHistoryDatabaseCache(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }

        public void TransactionBeginBatch()
        {
            dbTools.TransactionBeginBatch();
        }

        public void TransactionCommitBatch()
        {
            dbTools.TransactionCommitBatch(false);
        }


        #region Table: LocationSource
        public void WriteLocationHistorySource(string userAccount, string fileNamePath)
        {
            if (File.Exists(fileNamePath))
            {
                string sqlCommand =
                    "INSERT INTO LocationSource (UserAccount, FileDirectory, FileName, FileDateModified, FileDateImported) " +
                    "Values (@UserAccount, @FileDirectory, @FileName, @FileDateModified, @FileDateImported)";
                using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
                {
                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@UserAccount", userAccount);
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fileNamePath));
                    commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fileNamePath));
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(File.GetLastWriteTimeUtc(fileNamePath)));
                    commandDatabase.Parameters.AddWithValue("@FileDateImported", dbTools.ConvertFromDateTimeToDBVal(DateTime.UtcNow));
                    commandDatabase.ExecuteNonQuery();      // Execute the query
                }
            }
        }

        public void WriteLocationHistory(string userAccount, GoogleJsonLocations googleJsonLocations)
        {
            string sqlCommand =
                "INSERT OR IGNORE INTO LocationHistory (UserAccount, TimeStamp, Latitude, Longitude, Altitude, Accuracy) " +
                "Values (@UserAccount, @TimeStamp, @Latitude, @Longitude, @Altitude, @Accuracy) ";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
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
        }
        #endregion

        public Metadata FindLocationBasedOtherMediaFiles(DateTime? locationDateTime, DateTime? mediaDateTaken, DateTime? fileDateCreated, int acceptDiffrentSecound)
        {
            if (locationDateTime == null && mediaDateTaken == null && fileDateCreated == null) return null;

            string sqlCommand = "";

            if (locationDateTime != null)
                sqlCommand +=
                "(SELECT ABS(LocationDateTime - @LocationDateTime) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE LocationDateTime > @LocationDateTime AND LocationLatitude IS NOT NULL AND LocationLongitude IS NOT NULL " +
                "ORDER BY LocationDateTime LIMIT 1) "+
                "UNION SELECT * FROM " +
                "(SELECT ABS(LocationDateTime - @LocationDateTime) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE LocationDateTime < @LocationDateTime AND LocationLatitude IS NOT NULL AND LocationLongitude IS NOT NULL " +
                "ORDER BY LocationDateTime DESC LIMIT 1) ";
            
            if (mediaDateTaken != null) sqlCommand += 
                (sqlCommand == "" ? "" : "UNION SELECT * FROM ") +
                "(SELECT ABS(MediaDateTaken - @MediaDateTaken) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE MediaDateTaken > @MediaDateTaken AND LocationLatitude IS NOT NULL AND LocationLongitude IS NOT NULL " +
                "ORDER BY MediaDateTaken LIMIT 1 ) " +
                "UNION SELECT * FROM " +
                "(SELECT ABS(MediaDateTaken - @MediaDateTaken) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE MediaDateTaken < @MediaDateTaken AND LocationLatitude IS NOT NULL AND LocationLongitude IS NOT NULL " +
                "ORDER BY MediaDateTaken DESC LIMIT 1) ";

            if (fileDateCreated != null) sqlCommand +=
                (sqlCommand == "" ? "" : "UNION SELECT * FROM ") +
                "(SELECT ABS(FileDateCreated - @FileDateCreated) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE FileDateCreated > @FileDateCreated AND LocationLatitude IS NOT NULL AND LocationLongitude IS NOT NULL " +
                "ORDER BY FileDateCreated LIMIT 1) " +
                "UNION SELECT * FROM " +
                "(SELECT ABS(FileDateCreated - @FileDateCreated) AS TimeDistance, LocationLatitude, LocationLongitude FROM MediaMetadata " +
                "WHERE FileDateCreated < @FileDateCreated AND LocationLatitude IS NOT NULL AND LocationLongitude IS NOT NULL " +
                "ORDER BY FileDateCreated DESC LIMIT 1) ";
            
            sqlCommand = "SELECT * FROM " + sqlCommand +
                "ORDER BY TimeDistance " +
                "LIMIT 1";

            Metadata metadata = null;
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                if (locationDateTime != null) commandDatabase.Parameters.AddWithValue("@LocationDateTime", dbTools.ConvertFromDateTimeToDBVal(locationDateTime));
                if (mediaDateTaken != null) commandDatabase.Parameters.AddWithValue("@MediaDateTaken", dbTools.ConvertFromDateTimeToDBVal(mediaDateTaken));
                if (fileDateCreated != null) commandDatabase.Parameters.AddWithValue("@FileDateCreated", dbTools.ConvertFromDateTimeToDBVal(fileDateCreated));

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    
                    if (reader.Read())
                    {
                        long? timeDistance = dbTools.ConvertFromDBValLong(reader["TimeDistance"]);
                        float? locationLatitude = dbTools.ConvertFromDBValFloat(reader["LocationLatitude"]);
                        float? locationLongitude = dbTools.ConvertFromDBValFloat(reader["LocationLongitude"]);

                        if (timeDistance / 100000 < acceptDiffrentSecound)
                        {
                            metadata = new Metadata(MetadataBrokerType.GoogleLocationHistory);
                            metadata.LocationLatitude = locationLatitude;
                            metadata.LocationLongitude = locationLongitude;
                        }
                    }

                }
            }
            return metadata;
        }

        public Metadata FindLocationBasedOnTime(String userAccount, DateTime? datetime, int acceptDiffrentSecound)
        {
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
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
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
                        //UserAccount, TimeStamp, Latitude, Longitude, Altitude, Accuracy
                        minTimeStamp = (DateTime)dbTools.ConvertFromDBValDateTimeUtc(reader["TimeStamp"]); 
                        minLatitude = dbTools.ConvertFromDBValFloat(reader["Latitude"]);
                        minLongitude = dbTools.ConvertFromDBValFloat(reader["Longitude"]);
                        minAltitude = dbTools.ConvertFromDBValFloat(reader["Altitude"]);
                        minAccuracy = dbTools.ConvertFromDBValFloat(reader["Accuracy"]);
                    }
                    else
                    {
                        return null;
                    }

                    if (minTimeStamp == datetime)
                    {
                        metadata.LocationLatitude = minLatitude;
                        metadata.LocationLongitude = minLongitude;
                        metadata.LocationAltitude = minAltitude;
                    }


                    if (reader.Read())
                    {
                        //UserAccount, TimeStamp, Latitude, Longitude, Altitude, Accuracy
                        DateTime? maxTimeStamp = (DateTime)dbTools.ConvertFromDBValDateTimeUtc(reader["TimeStamp"]); 
                        float? maxLatitude = dbTools.ConvertFromDBValFloat(reader["Latitude"]);
                        float? maxLongitude = dbTools.ConvertFromDBValFloat(reader["Longitude"]);
                        float? maxAltitude = dbTools.ConvertFromDBValFloat(reader["Altitude"]);
                        float? maxAccuracy = dbTools.ConvertFromDBValFloat(reader["Accuracy"]);

                        Double lowDiffInSeconds = Math.Abs((minTimeStamp - datetime).Value.TotalSeconds);
                        Double highDiffInSeconds = Math.Abs((maxTimeStamp - datetime).Value.TotalSeconds);
                        Double totalDifInSeconds = lowDiffInSeconds + highDiffInSeconds;
                        if (Math.Min(lowDiffInSeconds, highDiffInSeconds) > acceptDiffrentSecound) 
                            return null;


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

                        return metadata;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }




    }
}

