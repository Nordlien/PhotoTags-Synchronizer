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

        
        //Created this use incase of switching DateTime format in Sqlite database
        /*
        ISO8601 string
            String comparison gives valid results
            Stores fraction seconds, up to three decimal digits
            Needs more storage space
            You will directly see its value when using a database browser
            Need for parsing for other uses
            "default current_timestamp" column modifier will store using this format
        Real number
            High precision regarding fraction seconds
            Longest time range
        Integer number
            Lowest storage space
            Quick operations
            Small time range
            Possible year 2038 problem
        */
        public const string SqliteDateTimeFormat = "INTEGER";

        public long ConvertFromDateTimeToDBVal(DateTime? dateTime)
        {
            if (dateTime == null) 
                return 0;
            return dateTime.Value.Ticks;
        }

        public DateTime? ConvertTicksToDateTime(object tickes)
        {
            if (tickes == null || tickes == DBNull.Value) return null;
            return new DateTime((long)tickes);
        }

        public DateTime? ConvertFromDBValDateTime(object tickes)
        {
            return ConvertTicksToDateTime(tickes);
        }



        public byte? ConvertFromDBValByte(object obj)
        {
#if MonoSqlite
            if (obj == null || obj == DBNull.Value) return (byte?)null;
            return (byte?)(long?)obj;
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

       
        public double? ConvertFromDBValDouble(object obj)
        {
            if (obj == null || obj == DBNull.Value) return (double?) null;

#if MonoSqlite
            //I used (double?)(float?) That gave sometimes wrong numbers
            return (double?)Math.Round((float)obj, 5); // (double?)double.Parse(((float)obj).ToString("G5"), System.Globalization.CultureInfo.CurrentCulture);
#else
                return (double?)obj;
#endif

        }

        public string ConvertFromDBValString(object obj)
        {
            if (obj == null || obj == DBNull.Value) return (string)null;
            return (string)obj;             
        }
#endregion

        
        
#region Connect Microsoft Phontos Database
        public void ConnectMicrosoftPhotosDatabase()  //TODO Move this out of here
        {

            if (ConnectionDatabase == null)
            {
                string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
                if (!Directory.Exists(databasePath))
                {
                    Directory.CreateDirectory(databasePath);
                }
                string destinationFile = Path.Combine(databasePath, "MediaDb.v1.sqlite");

                string sourceFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages\\Microsoft.Windows.Photos_8wekyb3d8bbwe\\LocalState\\MediaDb.v1.sqlite");
                
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
                if (ConnectionDatabase.State != System.Data.ConnectionState.Open)
                    ConnectionDatabase.Open();


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

            //Version=3;Pooling=True;Synchronous=Off;journal mode=Memory
            //Version=3;Synchronous=OFF;Journal Mode=Memory

            //Synchronous=OFF Journal Mode = MEMORY
            //https://www.connectionstrings.com/sqlite/ - SQLite connection strings
            //https://github.com/dotnet/corefx/issues/21530 
            //Execution result with .NET Framework 4.6.1 by creating Console App project (Release/Any CPU) in Visual Studio 2017 : 
            //average_time: 6.899ms
            //Execution result with .NET Core 1.1.0 by creating Console App project (Release/Any CPU) in Visual Studio 2017 :
            //average_time: 79.86ms
            //connectionDatabase = new SqliteConnection("Version=3;Cache Size=2000000;Pooling=False;Synchronous=Off;journal mode=Memory;data source=" + databaseFile);
#if MonoSqlite
            connectionDatabase = new SqliteConnection("data source=" + databaseFile + ";Synchronous=OFF;Journal Mode=Memory;"
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
                    "PersonalRatingPercent REAL, " +
                    "PersonalAuthor TEXT, " +
                    "CameraMake TEXT, " +
                    "CameraModel TEXT, " +
                    "MediaDateTaken " + SqliteDateTimeFormat + ", " +
                    "MediaWidth INTEGER, " +
                    "MediaHeight INTEGER, " +
                    "MediaOrientation INTEGER, " +
                    "MediaVideoLength REAL, " +
                    "LocationAltitude REAL, " +
                    "LocationLatitude REAL, " +
                    "LocationLongitude REAL, " +
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

                /*
                sqlCommand =
                    "CREATE UNIQUE INDEX MediaMetadataFileDirectoryFileName ON MediaMetadata " +
                    "(Broker, FileDirectory, FileName, FileDateModified)";
                using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }*/

                sqlCommand =
                        "CREATE TABLE MediaPersonalKeywords ( " +
                        "Broker                 INTEGER NOT NULL, " +
                        "FileDirectory          TEXT NOT NULL, " +
                        "FileName               TEXT NOT NULL, " +
                        "FileDateModified       " + SqliteDateTimeFormat + " NOT NULL, " +
                        "Keyword                TEXT NOT NULL, " +
                        "Confidence             REAL, " +
                        "UNIQUE (Broker, FileDirectory, FileName, FileDateModified, Keyword) )"; 
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }

                /*
                sqlCommand =
                    "CREATE UNIQUE INDEX MediaPersonalKeywordsFileDirectoryFileName ON MediaPersonalKeywords " +
                    "(Broker, FileDirectory, FileName, FileDateModified)";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }
                */

                sqlCommand =
                        "CREATE TABLE MediaPersonalRegions ( " +
                        "Broker                 INTEGER NOT NULL, " +
                        "FileDirectory          TEXT NOT NULL, " +
                        "FileName               TEXT NOT NULL, " +
                        "FileDateModified       " + SqliteDateTimeFormat + " NOT NULL, " +
                        "Type                   TEXT, " +
                        "Name                   TEXT, " +
                        "AreaX                  REAL, " +
                        "AreaY                  REAL, " +
                        "AreaWidth              REAL, " +
                        "AreaHeight             REAL, " +
                        "RegionStructureType    INTEGER, " +
                        "Thumbnail              BLOB, " +
                        "UNIQUE (Broker, FileDirectory, FileName, FileDateModified, Type, Name, AreaX, AreaY, AreaWidth, AreaHeight, RegionStructureType) )"; 

                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }

                /*
                sqlCommand =
                    "CREATE UNIQUE INDEX MediaPersonalRegionsFileDirectoryFileName ON MediaPersonalRegions " +
                    "(Broker, FileDirectory, FileName, FileDateModified)";

                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }
                */

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

                /*
                sqlCommand = 
                    "CREATE UNIQUE INDEX MediaThumbnailFileDirectoryFileNameSize ON MediaThumbnail " +
                    "(FileDirectory, FileName, FileDateModified)";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase))
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }                   
                */

                

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

                /*
                sqlCommand =
                    "CREATE INDEX MediaExiftoolTagsFileDirectoryFileName ON MediaExiftoolTags " +
                    "(FileDirectory, FileName, FileDateModified, Region, Command)";

                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase)) 
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }
                */
                
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

                /*
                sqlCommand = "CREATE INDEX MediaExiftoolTagsWarningFileDirectoryFileName  ON  MediaExiftoolTagsWarning " +
                    "(FileDirectory, FileName, FileDateModified)";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, this.connectionDatabase)) 
                {
                    commandDatabase.ExecuteNonQuery();                  // Execute the query
                }
                */

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
                    "Latitude           REAL, " +
                    "Longitude          REAL, " +
                    "Altitude           REAL, " +
                    "Accuracy           REAL, " +
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
                    "Latitude   REAL NOT NULL, " +
                    "Longitude  REAL NOT NULL, " +
                    "Name       TEXT NOT NULL, " +
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
                    "UserAccount    TEXT NOT NULL, " +
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
