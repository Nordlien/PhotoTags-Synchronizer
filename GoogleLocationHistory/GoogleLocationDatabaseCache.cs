#define MonoSqlite
#if MonoSqlite
using Mono.Data.Sqlite;
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
            dbTools.TransactionCommitBatch();
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
                    commandDatabase.Parameters.AddWithValue("@UserAccount", userAccount);
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fileNamePath));
                    commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fileNamePath));
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(File.GetLastWriteTimeUtc(fileNamePath)));
                    commandDatabase.Parameters.AddWithValue("@FileDateImported", dbTools.ConvertFromDateTimeToDBVal(DateTime.UtcNow));
                    commandDatabase.Prepare(); 
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
                commandDatabase.Parameters.AddWithValue("@UserAccount", userAccount);
                commandDatabase.Parameters.AddWithValue("@TimeStamp", dbTools.ConvertFromDateTimeToDBVal(googleJsonLocations.Timestamp));
                commandDatabase.Parameters.AddWithValue("@Latitude", googleJsonLocations.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", googleJsonLocations.Longitude);
                commandDatabase.Parameters.AddWithValue("@Altitude", googleJsonLocations.Altitude);
                commandDatabase.Parameters.AddWithValue("@Accuracy", googleJsonLocations.Accuracy);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }
        #endregion

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
                commandDatabase.Parameters.AddWithValue("@TimeStamp", dbTools.ConvertFromDateTimeToDBVal(datetime));
                commandDatabase.Parameters.AddWithValue("@UserAccount", userAccount);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    Metadata metadata = new Metadata(MetadataBrokerTypes.GoogleLocationHistory);
                    DateTime? minTimeStamp = null;
                    double? minLatitude = null;
                    double? minLongitude = null;
                    double? minAltitude = null;
                    double? minAccuracy = null;

                    if (reader.Read())
                    {
                        //UserAccount, TimeStamp, Latitude, Longitude, Altitude, Accuracy
                        minTimeStamp = (DateTime)dbTools.ConvertFromDBValDateTimeUtc(reader["TimeStamp"]); 
                        minLatitude = dbTools.ConvertFromDBValDouble(reader["Latitude"]);
                        minLongitude = dbTools.ConvertFromDBValDouble(reader["Longitude"]);
                        minAltitude = dbTools.ConvertFromDBValDouble(reader["Altitude"]);
                        minAccuracy = dbTools.ConvertFromDBValDouble(reader["Accuracy"]);
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
                        double? maxLatitude = dbTools.ConvertFromDBValDouble(reader["Latitude"]);
                        double? maxLongitude = dbTools.ConvertFromDBValDouble(reader["Longitude"]);
                        double? maxAltitude = dbTools.ConvertFromDBValDouble(reader["Altitude"]);
                        double? maxAccuracy = dbTools.ConvertFromDBValDouble(reader["Accuracy"]);

                        Double lowDiffInSeconds = Math.Abs((minTimeStamp - datetime).Value.TotalSeconds);
                        Double highDiffInSeconds = Math.Abs((maxTimeStamp - datetime).Value.TotalSeconds);
                        Double totalDifInSeconds = lowDiffInSeconds + highDiffInSeconds;
                        if (totalDifInSeconds > acceptDiffrentSecound) 
                            return null;


                        metadata.LocationAltitude =
                                ((minAltitude * (totalDifInSeconds - lowDiffInSeconds)) +
                                (maxAltitude * (totalDifInSeconds - highDiffInSeconds)))
                                / totalDifInSeconds;
                        metadata.LocationLatitude =
                                ((minLatitude * (totalDifInSeconds - lowDiffInSeconds)) +
                                (maxLatitude * (totalDifInSeconds - highDiffInSeconds)))
                                / totalDifInSeconds;
                        metadata.LocationLongitude =
                                ((minLongitude * (totalDifInSeconds - lowDiffInSeconds)) +
                                (maxLongitude * (totalDifInSeconds - highDiffInSeconds)))
                                / totalDifInSeconds;

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

