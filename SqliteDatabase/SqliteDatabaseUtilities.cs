#if MonoSqlite
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Timers;

namespace SqliteDatabase
{


    public class SqliteDatabaseUtilities // : IDisposable
    {
#if MonoSqlite
        private SqliteTransaction transactionHandler = null;
#else
        private SQLiteTransaction transactionHandler = null;
#endif

        private int transactionCount = 0;
        private bool transactionStarted = false;
        private Stopwatch transactionStopwatch = new Stopwatch();
        private Timer transactionTimer = new Timer();
        private int numberOfTransactionbeforeCommit = 10000;
        private int elapsedMillisecondsBeforeCommit = 5000;
        private object transactionLock = new object();
        public static int NumberOfDecimals { get; set; } = 5;
        public static int NumberOfDecimalsShort { get; set; } = 2;
        public const string SqliteDateTimeFormat = "INTEGER";
        public const string SqliteNumberFormat = "DECIMAL(10,5)";


        public SqliteDatabaseUtilities(DatabaseType type, int numberOfTransactionbeforeCommit, int elapsedMillisecondsBeforeCommit)
        {
            this.numberOfTransactionbeforeCommit = numberOfTransactionbeforeCommit;
            this.elapsedMillisecondsBeforeCommit = elapsedMillisecondsBeforeCommit;

            if (type == DatabaseType.SqliteMicrosoftPhotos)
            {
                ConnectMicrosoftPhotosDatabase();
            }
            else
            {
                ConnectSqliteCacheDatabase("metadata.db3");
            }
        }

        #region Transaction Begin and Commit



        public void TransactionBeginBatch()
        {
            lock (transactionLock)
            {
                if (transactionStarted) return;
            
                transactionCount = 0;
                transactionStarted = true;
                transactionStopwatch.Restart();
                transactionHandler = connectionDatabase.BeginTransaction();
                transactionTimer.Interval = elapsedMillisecondsBeforeCommit / 2;
                transactionTimer.Elapsed += TimerStatus_Elapsed;
            }
        }

        private void TimerStatus_Elapsed(object sender, ElapsedEventArgs e)
        {
            TransactionCommitBatch();
        }

        public void TransactionCommitBatch()
        {
            TransactionCommitBatch(false);
        }
        public void TransactionCommitBatch(bool forced)
        {
            lock (transactionLock)
            {
                if (transactionStarted)
                {
                
                    if (forced || transactionCount++ > numberOfTransactionbeforeCommit || transactionStopwatch.ElapsedMilliseconds > elapsedMillisecondsBeforeCommit)
                    {
                        transactionHandler.Commit();
                        transactionStarted = false;
                    }
                }
            }
        }

        public CommonDatabaseTransaction TransactionBegin(System.Data.IsolationLevel isolationLevel)
        {
            return new CommonDatabaseTransaction(connectionDatabase.BeginTransaction(isolationLevel));        
        }

        public void TransactionCommit(CommonDatabaseTransaction commonDatabaseTransaction)
        {
            commonDatabaseTransaction.DatabaseTransaction.Commit();    
        }


#endregion

        private string databasePath;
        private string databaseFile;

#if MonoSqlite
        private Mono.Data.Sqlite.SqliteConnection connectionDatabase;
#else
        private System.Data.SQLite.SQLiteConnection connectionDatabase;
#endif


#if MonoSqlite
        public Mono.Data.Sqlite.SqliteCommand SqliteCommand()
        {
            return new SqliteCommand(connectionDatabase);
        }
#else
        public SQLiteCommand SqliteCommand()
        {
            return new SQLiteCommand(connectionDatabase);
        }
#endif


#if MonoSqlite
        public SqliteConnection ConnectionDatabase { get => connectionDatabase; set => connectionDatabase = value; }
#else
        public SQLiteConnection ConnectionDatabase { get => connectionDatabase; set => connectionDatabase = value; }
#endif


#region Convert Object to Variable

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null) return (Image)null;
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        //DateTime format used by Microsoft Photos
        public DateTime? ConvertSecoundsSince1600ToDateTime(object dateTimeInSecond, DateTimeKind dateTimeKind)
        {
            DateTime? covertedDateTime;
            if (dateTimeInSecond != null && dateTimeInSecond != DBNull.Value)
            {
                DateTime newDateTime = new DateTime(new DateTime(1600, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc).AddSeconds(1).Ticks + (Int64)dateTimeInSecond, dateTimeKind);
                covertedDateTime = newDateTime.ToLocalTime();
            }
            else
            {
                covertedDateTime = new DateTime?();
            }
            return covertedDateTime;
        }

        

        public long ConvertFromDateTimeToDBVal(DateTime? dateTime)
        {
            if (dateTime == null) 
                return 0;
            return dateTime.Value.Ticks;
        }

        public DateTime? ConvertTicksToDateTimeLocal(object tickes)
        {
            if (tickes == null || tickes == DBNull.Value) return null;
            return new DateTime((long)tickes, DateTimeKind.Local);
        }

        public DateTime? ConvertTicksToDateTimeUtc(object tickes)
        {
            if (tickes == null || tickes == DBNull.Value) return null;
            return new DateTime((long)tickes, DateTimeKind.Utc);
        }

        public DateTime? ConvertFromDBValDateTimeLocal(object tickes)
        {
            return ConvertTicksToDateTimeLocal(tickes);
        }

        public DateTime? ConvertFromDBValDateTimeUtc(object tickes)
        {
            return ConvertTicksToDateTimeUtc(tickes);
        }



        public byte? ConvertFromDBValByte(object obj)
        {
#if MonoSqlite
            if (obj == null || obj == DBNull.Value) return (byte?)null;
            return (byte?)(long?)obj; //Was float in database, now database changed to byte, backward compablity 
#else
            if (obj == null || obj == DBNull.Value) return (byte?)null; 
            return (byte?)obj;
#endif
        }

        public byte[] ConvertFromDBValByteArray(object obj)
        {
            if (obj == null || obj == DBNull.Value) return (byte[])null;
            return (byte[])obj;
        }


        public int? ConvertFromDBValInt(object obj)
        {
#if MonoSqlite
            if (obj == null || obj == DBNull.Value) return (int?)null;
            return (int?)(long?)obj;
#else
            if (obj == null || obj == DBNull.Value) return (int?)null; 
            return (int?)(long?)obj;
#endif
        }



        public long? ConvertFromDBValLong(object obj)
        {
            if (obj == null || obj == DBNull.Value) return (long?)null; 
            return (long?)obj;
        }

       
        public float? ConvertFromDBValFloat(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;
#if MonoSqlite
            if (obj is float)
                return (float?)Math.Round((float)obj, NumberOfDecimals);
            if (obj is decimal)
                return (float?)Math.Round((decimal)obj, NumberOfDecimals);
            throw new Exception("Error in number format");
            //return null;
#else
                return (float?)obj;
#endif

        }

      

        public string ConvertFromDBValString(object obj)
        {
            if (obj == null || obj == DBNull.Value) return (string)null;
            return (string)obj;             
        }
#endregion

        public static string GetMicrosoftPhotosDatabaseBackupFile()
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            if (!Directory.Exists(databasePath))
            {
                Directory.CreateDirectory(databasePath);
            }
            return Path.Combine(databasePath, "MediaDb.v1.sqlite");
        }

        public static string GetMicrosoftPhotosDatabaseOriginalFile()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages\\Microsoft.Windows.Photos_8wekyb3d8bbwe\\LocalState\\MediaDb.v1.sqlite");
        }

        #region Connect Microsoft Phontos Database
        public void ConnectMicrosoftPhotosDatabase()  //TODO Move this out of here
        {

            if (ConnectionDatabase == null)
            {
                string destinationFile = GetMicrosoftPhotosDatabaseBackupFile();

                string sourceFile = GetMicrosoftPhotosDatabaseOriginalFile();
                try
                {
                    if (!File.Exists(destinationFile) || (File.GetLastWriteTime(sourceFile) >= File.GetLastWriteTime(destinationFile).AddSeconds(3600))) //Copy new only every hour
                            File.Copy(sourceFile, destinationFile, true);
                    
                }
                catch {}

#if MonoSqlite
                ConnectionDatabase = new SqliteConnection("Data Source=" +
                        destinationFile
                        //+ ";Version=3;Pooling=True;Synchronous=Off;Journal Mode=Off; Read Only = false;nolock=true;");
                        ); // +";Version=3;Pooling=True;Synchronous=Off;Journal Mode=Off; Read Only = true");               
#else
                ConnectionDatabase = new SQLiteConnection("Data Source=" +
                        destinationFile);

#endif
                Exception forwardException = null;
                try
                {

                    if (ConnectionDatabase.State != System.Data.ConnectionState.Open)
                        ConnectionDatabase.Open();
                } catch (Exception ex)
                {
                    forwardException = ex;                    
                    ConnectionDatabase.Close();
                    ConnectionDatabase.Dispose();
                    ConnectionDatabase = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers(); //Otherwise file be locked when try to delete
                }
                try
                {
                    //Failed copying the database file, delete the backup
                    if (forwardException != null)
                    {
                        File.Delete(SqliteDatabaseUtilities.GetMicrosoftPhotosDatabaseOriginalFile());
                    }
                }
                catch { }

                if (forwardException != null) throw forwardException;
            }
        }
#endregion

        public void ConnectPhotoTagsSynchronizerDatabase()
        {
            ConnectSqliteCacheDatabase("metadata.db3");
        }


#region Connect and create Metadata Database
        public void ConnectSqliteCacheDatabase(string databasename)
        {
            // This is the query which will create a new table in our database file with three columns. An auto increment column called "ID", and two NVARCHAR type columns with the names "Key" and "Value"

            databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            if (!Directory.Exists(databasePath))
            {
                Directory.CreateDirectory(databasePath);
            }

            databaseFile = Path.Combine(databasePath, databasename);

            bool doesDatabaseExist = File.Exists(databaseFile);
            if (!doesDatabaseExist)
            {

#if MonoSqlite
                SqliteConnection.CreateFile(databaseFile);        // Create the file which will be hosting our database
#else
                SQLiteConnection.CreateFile(databaseFile);        // Create the file which will be hosting our database
#endif

            }

#if MonoSqlite
            connectionDatabase = new SqliteConnection("data source=" + databaseFile + ";Synchronous=OFF;Journal Mode=Memory;Cache Size=20000;"
                            //+ ";Version=3;Pooling=True;Synchronous=Off;Journal Mode=Off; Read Only = false;nolock=true;");
                            ); // + ";Version=3;Pooling=True;Synchronous=Off;Journal Mode=Off; Read Only = false;nolock=false;");
#else
            connectionDatabase = new SQLiteConnection("data source=" + databaseFile
                + ";Version=3;Pooling=True;Synchronous=Off;Journal Mode=Memory;Read Only=False;Nolock=true;"
                );
#endif

            connectionDatabase.Open();                             // Open the connection to the database

            string sqlCommand;
            
            if (!doesDatabaseExist)
            {
                sqlCommand =
                    "CREATE TABLE MediaMetadata (" +
                    "Broker INTEGER NOT NULL, " +
                    "FileDirectory TEXT NOT NULL, " +
                    "FileName TEXT NOT NULL, " +
                    "FileDateModified " + SqliteDateTimeFormat + " NOT NULL, " +
                    "FileDateCreated " + SqliteDateTimeFormat + " NOT NULL, " +
                    "FileLastAccessed " + SqliteDateTimeFormat + " NOT NULL, " +
                    "FileSize INTEGER NOT NULL, " +
                    "FileMimeType TEXT, " +
                    "PersonalAlbum TEXT, " +
                    "PersonalTitle TEXT, " +
                    "PersonalDescription TEXT, " +
                    "PersonalComments TEXT, " +
                    "PersonalRatingPercent INTEGER, " +
                    "PersonalAuthor TEXT, " +
                    "CameraMake TEXT, " +
                    "CameraModel TEXT, " +
                    "MediaDateTaken " + SqliteDateTimeFormat + ", " +
                    "MediaWidth INTEGER, " +
                    "MediaHeight INTEGER, " +
                    "MediaOrientation INTEGER, " +
                    "MediaVideoLength " + SqliteNumberFormat + ", " +
                    "LocationAltitude " + SqliteNumberFormat + ", " +
                    "LocationLatitude " + SqliteNumberFormat + ", " +
                    "LocationLongitude " + SqliteNumberFormat + ", " +
                    "LocationDateTime " + SqliteDateTimeFormat + ", " +
                    "LocationName TEXT, " +
                    "LocationCountry TEXT, " +
                    "LocationCity TEXT, " +
                    "LocationState TEXT, " +
                    "RowChangedDated " + SqliteDateTimeFormat + ", " +
                    "UNIQUE (Broker, FileDirectory, FileName, FileDateModified) )";
                using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }

                sqlCommand =
                        "CREATE TABLE MediaPersonalKeywords ( " +
                        "Broker                 INTEGER NOT NULL, " +
                        "FileDirectory          TEXT NOT NULL, " +
                        "FileName               TEXT NOT NULL, " +
                        "FileDateModified       " + SqliteDateTimeFormat + " NOT NULL, " +
                        "Keyword                TEXT NOT NULL, " +
                        "Confidence             " + SqliteNumberFormat + ", " +
                        "UNIQUE (Broker, FileDirectory, FileName, FileDateModified, Keyword) )"; 
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }

                sqlCommand =
                        "CREATE TABLE MediaPersonalRegions ( " +
                        "Broker                 INTEGER NOT NULL, " +
                        "FileDirectory          TEXT NOT NULL, " +
                        "FileName               TEXT NOT NULL, " +
                        "FileDateModified       " + SqliteDateTimeFormat + " NOT NULL, " +
                        "Type                   TEXT, " +
                        "Name                   TEXT, " +
                        "AreaX                  " + SqliteNumberFormat + ", " +
                        "AreaY                  " + SqliteNumberFormat + ", " +
                        "AreaWidth              " + SqliteNumberFormat + ", " +
                        "AreaHeight             " + SqliteNumberFormat + ", " +
                        "RegionStructureType    INTEGER, " +
                        "Thumbnail              BLOB, " +
                        "UNIQUE (Broker, FileDirectory, FileName, FileDateModified, Type, Name, AreaX, AreaY, AreaWidth, AreaHeight, RegionStructureType) )"; 

                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }

                sqlCommand = "CREATE TABLE MediaThumbnail (" +
                    "FileDirectory          TEXT NOT NULL, " +
                    "FileName               TEXT NOT NULL, " +
                    "FileDateModified       " + SqliteDateTimeFormat + " NOT NULL, " +
                    "Image                  BLOB, " +
                    "UNIQUE (FileDirectory, FileName, FileDateModified) )";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }

                sqlCommand =
                    "CREATE TABLE MediaExiftoolTags ( " +
                    "FileDirectory          TEXT NOT NULL, " +
                    "FileName               TEXT NOT NULL, " +
                    "FileDateModified       " + SqliteDateTimeFormat + " NOT NULL, " +
                    "Region                 TEXT NOT NULL, " +
                    "Command                TEXT NOT NULL, " +
                    "Parameter              TEXT, " +
                    "UNIQUE (FileDirectory, FileName, FileDateModified, Region, Command, Parameter) )";

                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }
                
                sqlCommand = "CREATE TABLE MediaExiftoolTagsWarning ("+
	                "FileDirectory      TEXT NOT NULL, " +
                    "FileName           TEXT NOT NULL, " +
                    "FileDateModified 	" + SqliteDateTimeFormat + " NOT NULL, " +
	                "OldRegion 	        TEXT, " +
	                "OldCommand 	    TEXT, " +
	                "OldParameter  	    TEXT, " +
                    "NewRegion 	        TEXT NOT NULL, " +
                    "NewCommand 	    TEXT NOT NULL, " +
                    "NewParameter  	    TEXT NOT NULL, " +
                    "Warning 	        TEXT NOT NULL, " +
                    "UNIQUE (FileDirectory, FileName, FileDateModified, OldRegion, OldCommand, NewRegion, NewCommand) )";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase)) 
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }

                #region Location
                sqlCommand = "CREATE TABLE LocationSource (" +
                    "UserAccount        TEXT NOT NULL, " +
                    "FileDirectory      TEXT NOT NULL, " +
                    "FileName           TEXT NOT NULL, " +
                    "FileDateModified   " + SqliteDateTimeFormat + " NOT NULL, " +
                    "FileDateImported   " + SqliteDateTimeFormat + " NOT NULL, " +
                    "UNIQUE (UserAccount, FileDirectory, FileName, FileDateModified) )";

                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }

                sqlCommand = "CREATE TABLE LocationHistory ( " +
                    "UserAccount        TEXT NOT NULL, " +
                    "TimeStamp          " + SqliteDateTimeFormat + ", " +  
                    "Latitude           " + SqliteNumberFormat + ", " +
                    "Longitude          " + SqliteNumberFormat + ", " +
                    "Altitude           " + SqliteNumberFormat + ", " +
                    "Accuracy           " + SqliteNumberFormat + ", " +
                    "UNIQUE (UserAccount, TimeStamp) )";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }

                /*
                sqlCommand = "CREATE UNIQUE INDEX  LocationHistoryTimeStamp  ON  LocationHistory (TimeStamp)";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }
                */

                sqlCommand = "CREATE TABLE LocationName (" +
                    "Latitude  " + SqliteNumberFormat + " NOT NULL, " +
                    "Longitude  " + SqliteNumberFormat + " NOT NULL, " +
                    "Name       TEXT, " +
                    "City       TEXT, " +
                    "Province   TEXT, " +
                    "Country    TEXT, " +
                    "UNIQUE (Latitude, Longitude) )";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }
                
                /*
                sqlCommand = "CREATE UNIQUE INDEX LocationNameLatitudeLongitude ON LocationName " +
                    "(Latitude, Longitude)";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }
                */

                sqlCommand = "CREATE TABLE CameraOwner (" +
                    "CameraMake     TEXT, " +
                    "CameraModel    TEXT, " +
                    "UserAccount    TEXT, " +
                    "UNIQUE (CameraMake, CameraModel) )";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }
                #endregion
            }
        }
#endregion

#region Database Close
        public void DatabaseClose()
        {
            TransactionCommitBatch(true);
            try
            {
                if (this.ConnectionDatabase.State == System.Data.ConnectionState.Open)
                    this.ConnectionDatabase.Close();
            } catch { }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
#endregion

        


    }
}
