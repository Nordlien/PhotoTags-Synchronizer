#define MonoSqlite
#if MonoSqlite
using Mono.Data.Sqlite;
using NLog;
#else
using System.Data.SQLite;
#endif
using SqliteDatabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetadataLibrary
{
    public class ReadToCacheParameterRecordEventArgs : EventArgs
    {        
        public int MetadataCount { get; set; } = 0;
        public int RegionCount { get; set; } = 0;
        public int KeywordCount { get; set; } = 0;
        public bool Aborted { get; set; } = false;
    }

    public class ReadToCacheFileEntriesRecordEventArgs : EventArgs
    {

        public int HashQueue { get; set; } = 0;
        public int FileEntries { get; set; } = 0;
        public int MetadataCount { get; set; } = 0;
        public int RegionCount { get; set; } = 0;
        public int KeywordCount { get; set; } = 0;
        public bool Aborted { get; set; } = false;
        public bool InitCounter { get; set; } = true;

        public ReadToCacheFileEntriesRecordEventArgs()
        {

        }
        public ReadToCacheFileEntriesRecordEventArgs(ReadToCacheFileEntriesRecordEventArgs orginal) 
            : this(orginal.HashQueue, orginal.FileEntries, orginal.MetadataCount, orginal.RegionCount, orginal.KeywordCount, orginal.Aborted, false)
        {
        }
        
        public ReadToCacheFileEntriesRecordEventArgs(int hashQueue, int fileEntries, int metadataCount, int regionCount, int keywordCount, bool aborted, bool initCounter)
        {
            HashQueue = hashQueue;
            FileEntries = fileEntries;
            MetadataCount = metadataCount;
            RegionCount = regionCount;
            KeywordCount = keywordCount;
            Aborted = aborted;
            InitCounter = initCounter;
        } 
    }

    public class DeleteRecordEventArgs : EventArgs
    {
        public int HashQueue { get; set; } = 0;
        public string TableName { get; set; } = "";
        public int FileEntries { get; set; } = 0;
        public int Count { get; set; } = 0;
        public bool Aborted { get; set; } = false;
        public bool InitCounter { get; set; } = true;

        public DeleteRecordEventArgs()
        {
        }

        public DeleteRecordEventArgs(DeleteRecordEventArgs orginal) : this (orginal.HashQueue, orginal.TableName, orginal.FileEntries, orginal.Count, orginal.Aborted, false)
        {
        }

        public DeleteRecordEventArgs(int hashQueue, string tableName, int fileEntries, int count, bool aborted, bool initCounter)
        {
            HashQueue = hashQueue;
            TableName = tableName;
            FileEntries = fileEntries;
            Count = count;
            Aborted = aborted;
            InitCounter = initCounter;
        }
    }

    public class MetadataDatabaseCache
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static bool StopCaching { get; set; } = false;

        public delegate void ReadToCacheParameterRecordEvent(object sender, ReadToCacheParameterRecordEventArgs e);
        public event ReadToCacheParameterRecordEvent OnRecordReadToCacheParameter;

        public delegate void ReadToCacheRecordEvent(object sender, ReadToCacheFileEntriesRecordEventArgs e);
        public event ReadToCacheRecordEvent OnRecordReadToCache;

        public delegate void DeleteRecordEvent(object sender, DeleteRecordEventArgs e);
        public event DeleteRecordEvent OnDeleteRecord;

        private SqliteDatabaseUtilities dbTools;
        public MetadataDatabaseCache(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }
        public void TransactionBeginBatch()
        {
            dbTools.TransactionBeginBatch();
        }

        public void TransactionCommitBatch(bool force = false)
        {
            dbTools.TransactionCommitBatch(force);
        }

        #region Read (FileEntryBroker)
        /// <summary>
        /// Find metadata in database or cache and return the found metadata
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Metadata found, null if not found</returns>
        private Metadata Read(FileEntryBroker file)
        {
            List<FileEntryBroker> fileEntryBrokersToPutInCache = new List<FileEntryBroker>();
            fileEntryBrokersToPutInCache.Add(file);
            ReadToCache(fileEntryBrokersToPutInCache);
            return ReadMetadataFromCacheOnly(file);
            
        }
        #endregion

        #region ReadToCacheWebScraperDataSet
        public void ReadToCacheWebScraperDataSet(HashSet<FileEntry> filesFoundInDirectory)
        {
            DateTime? dataSetDateTime = GetWebScraperLastPackageDate();
            if (dataSetDateTime != null)
            {
                List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();
                foreach (FileEntry fileInfo in filesFoundInDirectory)
                {
                    fileEntryBrokers.Add(new FileEntryBroker(MetadataLibrary.MetadataDatabaseCache.WebScapingFolderName, fileInfo.FileName, (DateTime)dataSetDateTime, MetadataBrokerType.WebScraping));
                }
                ReadToCache(fileEntryBrokers);
            }
        }
        #endregion

        #region ReadToCache - List<FileEntry>, MetadataBrokerType
        public void ReadToCache(List<FileEntry> filesFoundInDirectory, MetadataBrokerType metadataBrokerType)
        {
            List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();
            foreach (FileEntry fileEntry in filesFoundInDirectory)
            {
                if (StopCaching) { StopCaching = false; return; }
                fileEntryBrokers.Add(new FileEntryBroker(fileEntry, metadataBrokerType));
            }
            ReadToCache(fileEntryBrokers);
        }
        #endregion

        #region ReadToCache - List<FileEntry>, MetadataBrokerType
        public void ReadToCache(HashSet<FileEntry> filesFoundInDirectory, MetadataBrokerType metadataBrokerType)
        {
            List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();
            foreach (FileEntry fileEntry in filesFoundInDirectory)
            {
                if (StopCaching) { StopCaching = false; return; }
                fileEntryBrokers.Add(new FileEntryBroker(fileEntry, metadataBrokerType));
            }
            ReadToCache(fileEntryBrokers);
        }
        #endregion

        #region ReadToCache - List<FileEntryBroker>
        private void ReadToCache(List<FileEntryBroker> fileEntriesBroker)
        {
            
            List<FileEntryBroker> fileEntryBrokersToPutInCache = new List<FileEntryBroker>();
            foreach (FileEntryBroker fileEntryBrokerToCheckInCache in fileEntriesBroker)
            {
                if (StopCaching) { StopCaching = false; return; }
                if (!IsMetadataInCache(fileEntryBrokerToCheckInCache)) fileEntryBrokersToPutInCache.Add(fileEntryBrokerToCheckInCache);
            }

            if (fileEntryBrokersToPutInCache.Count() == 0)
            {
                StopCaching = false;
                return;
            }

            ReadToCacheFileEntriesRecordEventArgs readToCacheFileEntriesRecordEventArgsInit = new ReadToCacheFileEntriesRecordEventArgs();
            readToCacheFileEntriesRecordEventArgsInit.HashQueue = fileEntriesBroker.GetHashCode();
            readToCacheFileEntriesRecordEventArgsInit.FileEntries = fileEntryBrokersToPutInCache.Count() * 3;
            readToCacheFileEntriesRecordEventArgsInit.Aborted = false;
            if (OnRecordReadToCache != null) OnRecordReadToCache(this, readToCacheFileEntriesRecordEventArgsInit);

            #region MediaMetadata
            string sqlCommand =
                "SELECT " +
                    "Broker, FileDirectory, FileName, FileSize, " +
                    "FileDateCreated, FileDateModified, FileLastAccessed, FileMimeType, " +
                    "PersonalTitle, PersonalAlbum, PersonalDescription, PersonalComments, PersonalRatingPercent,PersonalAuthor, " +
                    "CameraMake, CameraModel, " +
                    "MediaDateTaken, MediaWidth, MediaHeight, MediaOrientation, " +
                    "LocationAltitude, LocationLatitude, LocationLongitude, LocationDateTime, " +
                    "LocationName, LocationCountry, LocationCity, LocationState " +
                "FROM MediaMetadata WHERE (Broker & @Broker) = @Broker AND FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                foreach (FileEntryBroker fileEntryBroker in fileEntryBrokersToPutInCache)
                {
                    if (StopCaching)
                    {
                        ReadToCacheFileEntriesRecordEventArgs readToCacheFileEntriesRecordEventArgsAbort = new ReadToCacheFileEntriesRecordEventArgs(readToCacheFileEntriesRecordEventArgsInit);
                        readToCacheFileEntriesRecordEventArgsAbort.Aborted = true;
                        if (OnRecordReadToCache != null) OnRecordReadToCache(this, readToCacheFileEntriesRecordEventArgsAbort);
                        StopCaching = false; 
                        return;
                    }

                    readToCacheFileEntriesRecordEventArgsInit.MetadataCount++;
                    ReadToCacheFileEntriesRecordEventArgs readToCacheFileEntriesRecordEventArgs = new ReadToCacheFileEntriesRecordEventArgs(readToCacheFileEntriesRecordEventArgsInit);                   
                    if (OnRecordReadToCache != null) OnRecordReadToCache(this, readToCacheFileEntriesRecordEventArgs);

                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@Broker", (int)fileEntryBroker.Broker);
                    commandDatabase.Parameters.AddWithValue("@FileName", fileEntryBroker.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntryBroker.Directory);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntryBroker.LastWriteDateTime));

                    using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Metadata metadata = ReadMetadataFromCacheOnly(fileEntryBroker);
                            if (metadata == null) metadata = new Metadata(fileEntryBroker.Broker);
                            metadata.Broker = (MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"]);
                            metadata.FileDirectory = dbTools.ConvertFromDBValString(reader["FileDirectory"]);
                            metadata.FileName = dbTools.ConvertFromDBValString(reader["FileName"]);
                            metadata.FileSize = dbTools.ConvertFromDBValLong(reader["FileSize"]);
                            metadata.FileDateCreated = dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateCreated"]);
                            metadata.FileDateModified = dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]);
                            metadata.FileDateAccessed = dbTools.ConvertFromDBValDateTimeLocal(reader["FileLastAccessed"]);
                            metadata.FileMimeType = dbTools.ConvertFromDBValString(reader["FileMimeType"]);
                            metadata.PersonalAlbum = dbTools.ConvertFromDBValString(reader["PersonalAlbum"]);
                            metadata.PersonalTitle = dbTools.ConvertFromDBValString(reader["PersonalTitle"]);
                            metadata.PersonalDescription = dbTools.ConvertFromDBValString(reader["PersonalDescription"]);
                            metadata.PersonalComments = dbTools.ConvertFromDBValString(reader["PersonalComments"]);
                            metadata.PersonalRatingPercent = dbTools.ConvertFromDBValByte(reader["PersonalRatingPercent"]);
                            metadata.PersonalAuthor = dbTools.ConvertFromDBValString(reader["PersonalAuthor"]);
                            metadata.CameraMake = dbTools.ConvertFromDBValString(reader["CameraMake"]);
                            metadata.CameraModel = dbTools.ConvertFromDBValString(reader["CameraModel"]);
                            metadata.MediaDateTaken = dbTools.ConvertFromDBValDateTimeLocal(reader["MediaDateTaken"]);
                            metadata.MediaWidth = dbTools.ConvertFromDBValInt(reader["MediaWidth"]);
                            metadata.MediaHeight = dbTools.ConvertFromDBValInt(reader["MediaHeight"]);
                            metadata.MediaOrientation = dbTools.ConvertFromDBValInt(reader["MediaOrientation"]);
                            metadata.LocationAltitude = dbTools.ConvertFromDBValFloat(reader["LocationAltitude"]);
                            metadata.LocationLatitude = dbTools.ConvertFromDBValFloat(reader["LocationLatitude"]);
                            metadata.LocationLongitude = dbTools.ConvertFromDBValFloat(reader["LocationLongitude"]);
                            metadata.LocationDateTime = dbTools.ConvertFromDBValDateTimeUtc(reader["LocationDateTime"]);
                            metadata.LocationName = dbTools.ConvertFromDBValString(reader["LocationName"]);
                            metadata.LocationCountry = dbTools.ConvertFromDBValString(reader["LocationCountry"]);
                            metadata.LocationCity = dbTools.ConvertFromDBValString(reader["LocationCity"]);
                            metadata.LocationState = dbTools.ConvertFromDBValString(reader["LocationState"]);

                            MetadataCacheUpdate(fileEntryBroker, metadata);
                        } else
                        {
                            MetadataCacheUpdate(fileEntryBroker, null);
                        }
                    }
                }
            }
            #endregion

            #region MediaPersonalKeywords
            sqlCommand =
                   "SELECT " +
                       "Broker, FileDirectory, FileName, FileDateModified, Keyword, Confidence " +
                   "FROM MediaPersonalKeywords WHERE (Broker & @Broker) = @Broker AND FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                foreach (FileEntryBroker fileEntryBroker in fileEntryBrokersToPutInCache)
                {
                    if (StopCaching)
                    {
                        ReadToCacheFileEntriesRecordEventArgs readToCacheFileEntriesRecordEventArgsAbort = new ReadToCacheFileEntriesRecordEventArgs(readToCacheFileEntriesRecordEventArgsInit);
                        readToCacheFileEntriesRecordEventArgsAbort.Aborted = true;
                        if (OnRecordReadToCache != null) OnRecordReadToCache(this, readToCacheFileEntriesRecordEventArgsAbort);
                        StopCaching = false;
                        return;
                    }
                    readToCacheFileEntriesRecordEventArgsInit.KeywordCount++;
                    ReadToCacheFileEntriesRecordEventArgs readToCacheFileEntriesRecordEventArgs = new ReadToCacheFileEntriesRecordEventArgs(readToCacheFileEntriesRecordEventArgsInit);
                    
                    if (OnRecordReadToCache != null) OnRecordReadToCache(this, readToCacheFileEntriesRecordEventArgs);

                    Metadata metadata = ReadMetadataFromCacheOnly(fileEntryBroker);
                    if (metadata != null)
                    {
                        commandDatabase.Parameters.AddWithValue("@Broker", (int)fileEntryBroker.Broker);
                        commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fileEntryBroker.FileFullPath));
                        commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fileEntryBroker.FileFullPath));
                        commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntryBroker.LastWriteDateTime));

                        using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                metadata.PersonalKeywordTagsAddIfNotExists(
                                    new KeywordTag(
                                    dbTools.ConvertFromDBValString(reader["Keyword"]),
                                    (float)dbTools.ConvertFromDBValFloat(reader["Confidence"]))
                                    );

                            }
                        }
                    }
                }
            }
            #endregion

            #region MediaPersonalRegions
            sqlCommand =
                    "SELECT " +
                    "Broker, FileDirectory, FileName, FileDateModified, Type, " +
                    "Name, AreaX, AreaY, AreaWidth, AreaHeight, RegionStructureType, Thumbnail " +
                    "FROM MediaPersonalRegions " +
                    "WHERE (Broker & @Broker) = @Broker AND FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();

                foreach (FileEntryBroker fileEntryBroker in fileEntryBrokersToPutInCache)
                {
                    if (StopCaching)
                    {
                        ReadToCacheFileEntriesRecordEventArgs readToCacheFileEntriesRecordEventArgsAbort = new ReadToCacheFileEntriesRecordEventArgs(readToCacheFileEntriesRecordEventArgsInit);
                        readToCacheFileEntriesRecordEventArgsAbort.Aborted = true;
                        if (OnRecordReadToCache != null) OnRecordReadToCache(this, readToCacheFileEntriesRecordEventArgsAbort);
                        StopCaching = false;
                        return;
                    }

                    readToCacheFileEntriesRecordEventArgsInit.RegionCount++;
                    ReadToCacheFileEntriesRecordEventArgs readToCacheFileEntriesRecordEventArgs = new ReadToCacheFileEntriesRecordEventArgs(readToCacheFileEntriesRecordEventArgsInit);                    
                    if (OnRecordReadToCache != null) OnRecordReadToCache(this, readToCacheFileEntriesRecordEventArgs);

                    Metadata metadata = ReadMetadataFromCacheOnly(fileEntryBroker);
                    if (metadata != null)
                    {
                        commandDatabase.Parameters.AddWithValue("@Broker", (int)fileEntryBroker.Broker);
                        commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fileEntryBroker.FileFullPath));
                        commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fileEntryBroker.FileFullPath));
                        commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntryBroker.LastWriteDateTime));

                        using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RegionStructure region = new RegionStructure();
                                region.Type = dbTools.ConvertFromDBValString(reader["Type"]);
                                region.Name = dbTools.ConvertFromDBValString(reader["Name"]);
                                region.AreaX = (float)dbTools.ConvertFromDBValFloat(reader["AreaX"]);
                                region.AreaY = (float)dbTools.ConvertFromDBValFloat(reader["AreaY"]);
                                region.AreaWidth = (float)dbTools.ConvertFromDBValFloat(reader["AreaWidth"]);
                                region.AreaHeight = (float)dbTools.ConvertFromDBValFloat(reader["AreaHeight"]);
                                region.RegionStructureType = (RegionStructureTypes)(int)dbTools.ConvertFromDBValInt(reader["RegionStructureType"]);
                                region.Thumbnail = dbTools.ByteArrayToImage(dbTools.ConvertFromDBValByteArray(reader["Thumbnail"]));
                                metadata.PersonalRegionListAddIfNotExists(region);
                            }
                        }
                    }
                }
            }
            #endregion 

            ReadToCacheFileEntriesRecordEventArgs readToCacheFileEntriesRecordEventArgsEnd = new ReadToCacheFileEntriesRecordEventArgs(readToCacheFileEntriesRecordEventArgsInit);
            readToCacheFileEntriesRecordEventArgsEnd.Aborted = true;
            if (OnRecordReadToCache != null) OnRecordReadToCache(this, readToCacheFileEntriesRecordEventArgsEnd);
        }
        #endregion

        #region ReadToCache - Folder
        public void ReadToCacheAllMetadatas(string folder, MetadataBrokerType metadataBrokerType) //Hack to read data to cache and the database worked much faster after this
        {
            if (StopCaching) { StopCaching = false; return; }
            ReadToCacheWhereParameters(metadataBrokerType, folder, null, null, true);
        }
        #endregion

        #region ReadToCache - All Metadatas
        public void ReadToCacheAllMetadatas() //Hack to read data to cache and the database worked much faster after this
        {
            if (StopCaching) { StopCaching = false; return; }
            ReadToCacheWhereParameters(MetadataBrokerType.Empty, null, null, null, true);
        }
        #endregion

        #region ReadToCache - All WebScarping DataSets
        public void ReadToCacheWebScarpingAllDataSets()
        {
            if (StopCaching) { StopCaching = false; return; }
            DateTime? dataSetDateTime = GetWebScraperLastPackageDate();
            if (dataSetDateTime != null) ReadToCacheWhereParameters(MetadataBrokerType.WebScraping, MetadataLibrary.MetadataDatabaseCache.WebScapingFolderName, null, dataSetDateTime, true);
        }
        #endregion

        #region ReadLot

        private static List<ReadToCacheParameters> readToCacheParamtersCached = new List<ReadToCacheParameters>();

        #region class ReadToCacheParameters - ReadToCache 
        private class ReadToCacheParameters : IComparable<ReadToCacheParameters>, IEquatable<ReadToCacheParameters>
        {
            public ReadToCacheParameters()
            {
            }

            public ReadToCacheParameters(MetadataBrokerType metadataBrokerType, string folder, string filename, DateTime? fileDateModified)
            {
                MetadataBrokerType = metadataBrokerType;
                Folder = folder;
                Filename = filename;
                FileDateModified = fileDateModified;
            }

            public MetadataBrokerType MetadataBrokerType { get; set; }
            public string Folder { get; set; }
            public string Filename { get; set; }
            public DateTime? FileDateModified { get; set; }

            public int CompareTo(ReadToCacheParameters other)
            {
                int compare = MetadataBrokerType.CompareTo(other.MetadataBrokerType);  

                if (Folder == null)
                {
                    if (other.Folder != null) compare = -1;                 //this.Fullname ==  null and other.Fullname NOT null
                }
                else if (other.Folder == null) compare = 1;                 //this.Fullname NOT null and other.Fullname ==  null                
                else compare = Folder.CompareTo(other.Folder);  //both != NULL, then do normale string compare

                if (Filename == null)
                {
                    if (other.Filename != null) compare = -1;                 //this.Fullname ==  null and other.Fullname NOT null
                }
                else if (other.Filename == null) compare = 1;                 //this.Fullname NOT null and other.Fullname ==  null                
                else compare = Filename.CompareTo(other.Filename);  //both != NULL, then do normale string compare

                if (FileDateModified == null)
                {
                    if (other.FileDateModified != null) compare = -1;                 //this.Fullname ==  null and other.Fullname NOT null
                }
                else if (other.FileDateModified == null) compare = 1;                 //this.Fullname NOT null and other.Fullname ==  null                
                else compare = ((DateTime)FileDateModified).CompareTo((DateTime)other.FileDateModified);  //both != NULL, then do normale string compare

                return compare;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as ReadToCacheParameters);
            }

            public bool Equals(ReadToCacheParameters other)
            {
                return other is ReadToCacheParameters paramters &&
                       MetadataBrokerType == paramters.MetadataBrokerType &&
                       Folder == paramters.Folder &&
                       Filename == paramters.Filename &&
                       FileDateModified == paramters.FileDateModified;
            }

            public override int GetHashCode()
            {
                int hashCode = 262226170;
                hashCode = hashCode * -1521134295 + MetadataBrokerType.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Folder);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Filename);
                hashCode = hashCode * -1521134295 + FileDateModified.GetHashCode();
                return hashCode;
            }

            public static bool operator ==(ReadToCacheParameters left, ReadToCacheParameters right)
            {
                return EqualityComparer<ReadToCacheParameters>.Default.Equals(left, right);
            }

            public static bool operator !=(ReadToCacheParameters left, ReadToCacheParameters right)
            {
                return !(left == right);
            }
        }
        #endregion

        #region ReadToCache - ReadToCacheWhereParameters
        public void ReadToCacheWhereParameters(MetadataBrokerType metadataBrokerType, string folder, string filename, DateTime? fileDateModified, bool readDataIntoCache = true) //Hack to read data to cache and the database worked much faster after this
        {
            if (StopCaching) { StopCaching = false; return; }
            ReadToCacheParameters readToCacheParamters = new ReadToCacheParameters(metadataBrokerType, folder, filename, fileDateModified);
            if (readToCacheParamtersCached.Contains(readToCacheParamters)) return;
            if (readDataIntoCache) readToCacheParamtersCached.Add(readToCacheParamters);

            string sqlWhere = "";
            if (metadataBrokerType != MetadataBrokerType.Empty) sqlWhere += (sqlWhere == "" ? "" : " AND ") + "(Broker & @Broker) = @Broker";
            if (folder != null) sqlWhere += (sqlWhere == "" ? "" : " AND ") + "FileDirectory = @FileDirectory";
            if (filename != null) sqlWhere += (sqlWhere == "" ? "" : " AND ") + "FileName = @FileName";
            if (fileDateModified != null) sqlWhere += (sqlWhere == "" ? "" : " AND ") + "FileDateModified = @FileDateModified";
            sqlWhere = (sqlWhere == "" ? "" : " WHERE ") + sqlWhere;

            #region MediaMetadata
            string sqlCommand =
                "SELECT " +
                    "Broker, FileDirectory, FileName, FileSize, " +
                    "FileDateCreated, FileDateModified, FileLastAccessed, FileMimeType, " +
                    "PersonalTitle, PersonalAlbum, PersonalDescription, PersonalComments, PersonalRatingPercent,PersonalAuthor, " +
                    "CameraMake, CameraModel, " +
                    "MediaDateTaken, MediaWidth, MediaHeight, MediaOrientation, " +
                    "LocationAltitude, LocationLatitude, LocationLongitude, LocationDateTime, " +
                    "LocationName, LocationCountry, LocationCity, LocationState " +
                "FROM MediaMetadata " + sqlWhere;

            ReadToCacheParameterRecordEventArgs readRecordEventArgs = new ReadToCacheParameterRecordEventArgs();

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                if (metadataBrokerType != MetadataBrokerType.Empty) commandDatabase.Parameters.AddWithValue("@Broker", metadataBrokerType);
                if (folder != null) commandDatabase.Parameters.AddWithValue("@FileDirectory", folder);
                if (filename != null) commandDatabase.Parameters.AddWithValue("@FileName", filename);
                if (fileDateModified != null) commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileDateModified));
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (StopCaching) { 
                            readRecordEventArgs.Aborted = true;
                            if (OnRecordReadToCacheParameter != null) OnRecordReadToCacheParameter(this, readRecordEventArgs); 
                            StopCaching = false; return; 
                        }
                        readRecordEventArgs.MetadataCount++;
                        if (OnRecordReadToCacheParameter != null) OnRecordReadToCacheParameter(this, readRecordEventArgs);

                        
                        if (readDataIntoCache)
                        {
                            Metadata metadata = new Metadata(metadataBrokerType);

                            metadata.Broker = (MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"]);
                            metadata.FileDirectory = dbTools.ConvertFromDBValString(reader["FileDirectory"]);
                            metadata.FileName = dbTools.ConvertFromDBValString(reader["FileName"]);
                            metadata.FileSize = dbTools.ConvertFromDBValLong(reader["FileSize"]);
                            metadata.FileDateCreated = dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateCreated"]);
                            metadata.FileDateModified = dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]);
                            metadata.FileDateAccessed = dbTools.ConvertFromDBValDateTimeLocal(reader["FileLastAccessed"]);
                            metadata.FileMimeType = dbTools.ConvertFromDBValString(reader["FileMimeType"]);
                            metadata.PersonalAlbum = dbTools.ConvertFromDBValString(reader["PersonalAlbum"]);
                            metadata.PersonalTitle = dbTools.ConvertFromDBValString(reader["PersonalTitle"]);
                            metadata.PersonalDescription = dbTools.ConvertFromDBValString(reader["PersonalDescription"]);
                            metadata.PersonalComments = dbTools.ConvertFromDBValString(reader["PersonalComments"]);
                            metadata.PersonalRatingPercent = dbTools.ConvertFromDBValByte(reader["PersonalRatingPercent"]);
                            metadata.PersonalAuthor = dbTools.ConvertFromDBValString(reader["PersonalAuthor"]);
                            metadata.CameraMake = dbTools.ConvertFromDBValString(reader["CameraMake"]);
                            metadata.CameraModel = dbTools.ConvertFromDBValString(reader["CameraModel"]);
                            metadata.MediaDateTaken = dbTools.ConvertFromDBValDateTimeLocal(reader["MediaDateTaken"]);
                            metadata.MediaWidth = dbTools.ConvertFromDBValInt(reader["MediaWidth"]);
                            metadata.MediaHeight = dbTools.ConvertFromDBValInt(reader["MediaHeight"]);
                            metadata.MediaOrientation = dbTools.ConvertFromDBValInt(reader["MediaOrientation"]);
                            metadata.LocationAltitude = dbTools.ConvertFromDBValFloat(reader["LocationAltitude"]);
                            metadata.LocationLatitude = dbTools.ConvertFromDBValFloat(reader["LocationLatitude"]);
                            metadata.LocationLongitude = dbTools.ConvertFromDBValFloat(reader["LocationLongitude"]);
                            metadata.LocationDateTime = dbTools.ConvertFromDBValDateTimeUtc(reader["LocationDateTime"]);
                            metadata.LocationName = dbTools.ConvertFromDBValString(reader["LocationName"]);
                            metadata.LocationCountry = dbTools.ConvertFromDBValString(reader["LocationCountry"]);
                            metadata.LocationCity = dbTools.ConvertFromDBValString(reader["LocationCity"]);
                            metadata.LocationState = dbTools.ConvertFromDBValString(reader["LocationState"]);

                            MetadataCacheUpdate(metadata.FileEntryBroker, metadata);
                        }
                    }
                }
            }
            #endregion

            #region MediaPersonalKeywords
            sqlCommand =
                   "SELECT " +
                       "Broker, FileDirectory, FileName, FileDateModified, Keyword, Confidence " +
                   "FROM MediaPersonalKeywords" + sqlWhere;

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                if (metadataBrokerType != MetadataBrokerType.Empty) commandDatabase.Parameters.AddWithValue("@Broker", metadataBrokerType);
                if (folder != null) commandDatabase.Parameters.AddWithValue("@FileDirectory", folder);
                if (filename != null) commandDatabase.Parameters.AddWithValue("@FileName", filename);
                if (fileDateModified != null) commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileDateModified));
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (StopCaching)
                        {
                            readRecordEventArgs.Aborted = true;
                            if (OnRecordReadToCacheParameter != null) OnRecordReadToCacheParameter(this, readRecordEventArgs);
                            StopCaching = false; return;
                        }
                        readRecordEventArgs.KeywordCount++;
                        if (OnRecordReadToCacheParameter != null) OnRecordReadToCacheParameter(this, readRecordEventArgs);

                        if (readDataIntoCache)
                        {
                            FileEntryBroker fileEntryBroker = new FileEntryBroker(
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]),
                            (MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"])
                            );

                            Metadata metadata = ReadMetadataFromCacheOnly(fileEntryBroker);

                            if (metadata != null)
                            {
                                metadata.PersonalKeywordTagsAddIfNotExists(
                                    new KeywordTag(
                                    dbTools.ConvertFromDBValString(reader["Keyword"]),
                                    (float)dbTools.ConvertFromDBValFloat(reader["Confidence"]))
                                    );
                            }
                        }
                    }
                }
            }
            #endregion

            #region MediaPersonalRegions
            sqlCommand =
                    "SELECT " +
                    "Broker, FileDirectory, FileName, FileDateModified, Type, " +
                    "Name, AreaX, AreaY, AreaWidth, AreaHeight, RegionStructureType, Thumbnail " +
                    "FROM MediaPersonalRegions " + sqlWhere;
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                if (metadataBrokerType != MetadataBrokerType.Empty) commandDatabase.Parameters.AddWithValue("@Broker", metadataBrokerType);
                if (folder != null) commandDatabase.Parameters.AddWithValue("@FileDirectory", folder);
                if (filename != null) commandDatabase.Parameters.AddWithValue("@FileName", filename);
                if (fileDateModified != null) commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileDateModified));
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (StopCaching)
                        {
                            readRecordEventArgs.Aborted = true;
                            if (OnRecordReadToCacheParameter != null) OnRecordReadToCacheParameter(this, readRecordEventArgs);
                            StopCaching = false; return;
                        }
                        readRecordEventArgs.RegionCount++;
                        if (OnRecordReadToCacheParameter != null) OnRecordReadToCacheParameter(this, readRecordEventArgs);

                        if (readDataIntoCache)
                        {
                            FileEntryBroker fileEntryBroker = new FileEntryBroker(
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]),
                            (MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"])
                            );

                            Metadata metadata = ReadMetadataFromCacheOnly(fileEntryBroker);

                            if (metadata != null)
                            {
                                RegionStructure region = new RegionStructure();
                                region.Type = dbTools.ConvertFromDBValString(reader["Type"]);
                                string name = dbTools.ConvertFromDBValString(reader["Name"]);
                                region.Name = name;
                                float? floatNull;
                                floatNull = dbTools.ConvertFromDBValFloat(reader["AreaX"]);
                                region.AreaX = (floatNull == null ? 0 : (float)floatNull);
                                if (floatNull == null)
                                    Logger.Error("AreaX is NULL, set to 0"); //This somehow got NULL, maybe after crash, need to debug, problem was gone after re-read metadata, need to find why occure

                                floatNull = dbTools.ConvertFromDBValFloat(reader["AreaY"]);
                                region.AreaY = (floatNull == null ? 0 : (float)floatNull);
                                if (floatNull == null)
                                    Logger.Error("AreaX is NULL, set to 0"); //This somehow got NULL, maybe after crash, need to debug, problem was gone after re-read metadata, need to find why occure

                                floatNull = dbTools.ConvertFromDBValFloat(reader["AreaWidth"]);
                                region.AreaWidth = (floatNull == null ? 0 : (float)floatNull);
                                if (floatNull == null)
                                    Logger.Error("AreaX is NULL, set to 0"); //This somehow got NULL, maybe after crash, need to debug, problem was gone after re-read metadata, need to find why occure

                                floatNull = dbTools.ConvertFromDBValFloat(reader["AreaHeight"]);
                                region.AreaHeight = (floatNull == null ? 0 : (float)floatNull);
                                if (floatNull == null)
                                    Logger.Error("AreaX is NULL, set to 0"); //This somehow got NULL, maybe after crash, need to debug, problem was gone after re-read metadata, need to find why occure

                                region.RegionStructureType = (RegionStructureTypes)(int)dbTools.ConvertFromDBValInt(reader["RegionStructureType"]);
                                region.Thumbnail = dbTools.ByteArrayToImage(dbTools.ConvertFromDBValByteArray(reader["Thumbnail"]));
                                metadata.PersonalRegionListAddIfNotExists(region);
                            }
                        }
                    }
                }
            }
            #endregion 
        }
        #endregion 

        #endregion

        #region Write - Metadata
        public void Write(Metadata metadata)
        {
            if (metadata == null) throw new Exception("Error in DatabaseCache. metaData is Null. Error in code");


            dbTools.TransactionBeginBatch();

            MetadataCacheUpdate(metadata.FileEntryBroker, metadata);

            string sqlCommand =
                "INSERT INTO MediaMetadata (" +
                    "Broker, FileDirectory, FileName, FileSize, " +
                    "FileDateCreated, FileDateModified, FileLastAccessed, FileMimeType, " +
                    "PersonalTitle, PersonalAlbum, PersonalDescription, PersonalComments, PersonalRatingPercent,PersonalAuthor, " +
                    "CameraMake, CameraModel, " +
                    "MediaDateTaken, MediaWidth, MediaHeight, MediaOrientation, MediaVideoLength, " +
                    "LocationAltitude, LocationLatitude, LocationLongitude, LocationDateTime, " +
                    "LocationName, LocationCountry, LocationCity, LocationState, RowChangedDated) " +
                "Values (" +
                    "@Broker, @FileDirectory, @FileName, @FileSize, " +
                    "@FileDateCreated, @FileDateModified, @FileLastAccessed, @FileMimeType, " +
                    "@PersonalTitle, @PersonalAlbum, @PersonalDescription, @PersonalComments, @PersonalRatingPercent, @PersonalAuthor, " +
                    "@CameraMake, @CameraModel,  " +
                    "@MediaDateTaken, @MediaWidth, @MediaHeight, @MediaOrientation, @MediaVideoLength, " +
                    "@LocationAltitude, @LocationLatitude, @LocationLongitude, @LocationDateTime, " +
                    "@LocationName, @LocationCountry, @LocationCity, @LocationState, @RowChangedDated" +
                ")";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadata.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", metadata.FileDirectory);
                commandDatabase.Parameters.AddWithValue("@FileName", metadata.FileName);
                commandDatabase.Parameters.AddWithValue("@FileSize", metadata.FileSize);
                commandDatabase.Parameters.AddWithValue("@FileDateCreated", dbTools.ConvertFromDateTimeToDBVal(metadata.FileDateCreated));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(metadata.FileDateModified));
                commandDatabase.Parameters.AddWithValue("@FileLastAccessed", dbTools.ConvertFromDateTimeToDBVal(metadata.FileDateAccessed));
                commandDatabase.Parameters.AddWithValue("@FileMimeType", metadata.FileMimeType);
                commandDatabase.Parameters.AddWithValue("@PersonalTitle", metadata.PersonalTitle);
                commandDatabase.Parameters.AddWithValue("@PersonalAlbum", metadata.PersonalAlbum);
                commandDatabase.Parameters.AddWithValue("@PersonalDescription", metadata.PersonalDescription);
                commandDatabase.Parameters.AddWithValue("@PersonalComments", metadata.PersonalComments);
                commandDatabase.Parameters.AddWithValue("@PersonalRatingPercent", metadata.PersonalRatingPercent);
                commandDatabase.Parameters.AddWithValue("@PersonalAuthor", metadata.PersonalAuthor);
                commandDatabase.Parameters.AddWithValue("@CameraMake", metadata.CameraMake);
                commandDatabase.Parameters.AddWithValue("@CameraModel", metadata.CameraModel);
                if (metadata.MediaDateTaken == null)
                    commandDatabase.Parameters.AddWithValue("@MediaDateTaken", null);
                else
                    commandDatabase.Parameters.AddWithValue("@MediaDateTaken", dbTools.ConvertFromDateTimeToDBVal(metadata.MediaDateTaken));
                commandDatabase.Parameters.AddWithValue("@MediaWidth", metadata.MediaWidth);
                commandDatabase.Parameters.AddWithValue("@MediaHeight", metadata.MediaHeight);
                commandDatabase.Parameters.AddWithValue("@MediaOrientation", metadata.MediaOrientation);
                commandDatabase.Parameters.AddWithValue("@MediaVideoLength", metadata.MediaVideoLength);
                commandDatabase.Parameters.AddWithValue("@LocationAltitude", metadata.LocationAltitude);
                commandDatabase.Parameters.AddWithValue("@LocationLatitude", metadata.LocationLatitude);
                commandDatabase.Parameters.AddWithValue("@LocationLongitude", metadata.LocationLongitude);
                if (metadata.LocationDateTime.HasValue)
                    commandDatabase.Parameters.AddWithValue("@LocationDateTime", dbTools.ConvertFromDateTimeToDBVal(metadata.LocationDateTime));
                else
                    commandDatabase.Parameters.AddWithValue("@LocationDateTime", DBNull.Value);
                commandDatabase.Parameters.AddWithValue("@LocationName", metadata.LocationName);
                commandDatabase.Parameters.AddWithValue("@LocationCountry", metadata.LocationCountry);
                commandDatabase.Parameters.AddWithValue("@LocationCity", metadata.LocationCity);
                commandDatabase.Parameters.AddWithValue("@LocationState", metadata.LocationState);
                commandDatabase.Parameters.AddWithValue("@RowChangedDated", dbTools.ConvertFromDateTimeToDBVal(DateTime.Now));
;
                //commandDatabase.ExecuteNonQuery();
                if (commandDatabase.ExecuteNonQuery() == -1)
                {
                    Logger.Warn("Delete MediaMetadata and sub data due to previous application crash for file: " + metadata.FileFullPath);
                    //Delete all extries due to crash.
                    DeleteFileEntryFromMediaMetadata(metadata.FileEntryBroker);
                    DeleteFileEntryFromMediaPersonalKeywords(metadata.FileEntryBroker);
                    DeleteFileEntryFromMediaPersonalRegions(metadata.FileEntryBroker);
                    commandDatabase.ExecuteNonQuery();
                }
            }

            sqlCommand =
                "INSERT INTO MediaPersonalKeywords (" +
                    "Broker, FileDirectory, FileName, FileDateModified, Keyword, Confidence " +
                ") Values (" +
                    "@Broker, @FileDirectory, @FileName, @FileDateModified, @Keyword, @Confidence)";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                foreach (KeywordTag tag in metadata.PersonalKeywordTags)
                {
                    commandDatabase.Parameters.AddWithValue("@Broker", metadata.Broker);
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", metadata.FileDirectory);
                    commandDatabase.Parameters.AddWithValue("@FileName", metadata.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(metadata.FileDateModified));
                    commandDatabase.Parameters.AddWithValue("@Keyword", tag.Keyword);
                    commandDatabase.Parameters.AddWithValue("@Confidence", tag.Confidence);
                    
                    if (commandDatabase.ExecuteNonQuery() == -1)
                    {
                        Logger.Warn("Delete MediaPersonalKeywords data due to previous application crash for file: " + metadata.FileFullPath);
                        //Delete all extries due to crash.
                        //DeleteFileEntryFromMediaPersonalKeywords(metadata.FileEntryBroker);
                        commandDatabase.ExecuteNonQuery();
                    }
                }
            }

            sqlCommand =
                "INSERT INTO MediaPersonalRegions (" +
                    "Broker, FileDirectory, FileName, FileDateModified, Type, Name, " +
                    "AreaX, AreaY, AreaWidth, AreaHeight, RegionStructureType, Thumbnail " +
                ") Values (" +
                    "@Broker, @FileDirectory, @FileName, @FileDateModified, @Type, @Name, " +
                    "@AreaX, @AreaY, @AreaWidth, @AreaHeight, @RegionStructureType, @Thumbnail " +
                    ")";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                foreach (RegionStructure region in metadata.PersonalRegionList)
                {
                    PersonalRegionNameCountCacheUpdated(metadata.Broker, region.Name);
                    commandDatabase.Parameters.AddWithValue("@Broker", (int)metadata.Broker);
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", metadata.FileDirectory);
                    commandDatabase.Parameters.AddWithValue("@FileName", metadata.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(metadata.FileDateModified));
                    commandDatabase.Parameters.AddWithValue("@Type", region.Type);
                    commandDatabase.Parameters.AddWithValue("@Name", region.Name);
                    commandDatabase.Parameters.AddWithValue("@AreaX", region.AreaX);
                    commandDatabase.Parameters.AddWithValue("@AreaY", region.AreaY);
                    commandDatabase.Parameters.AddWithValue("@AreaWidth", region.AreaWidth);
                    commandDatabase.Parameters.AddWithValue("@AreaHeight", region.AreaHeight);
                    commandDatabase.Parameters.AddWithValue("@RegionStructureType", region.RegionStructureType);
                    if (region.Thumbnail == null) commandDatabase.Parameters.AddWithValue("@Thumbnail", DBNull.Value);
                    else commandDatabase.Parameters.AddWithValue("@Thumbnail", dbTools.ImageToByteArray(region.Thumbnail));
                    
                    if (commandDatabase.ExecuteNonQuery() == -1)
                    {
                        Logger.Warn("Delete MediaPersonalRegions data due to previous application crash for file: " + metadata.FileFullPath);
                        //Delete all extries due to crash.
                        //DeleteFileEntryFromMediaPersonalRegions(metadata.FileEntryBroker);
                        commandDatabase.ExecuteNonQuery();
                    }
                }
            }

            dbTools.TransactionCommitBatch(false);
        }
        #endregion

        #region Updated - Region - UpdateRegionThumbnail
        /// <summary>
        /// Updated Region data for Give media file
        /// </summary>
        /// <param name="fileEntryBroker">Mediafile data will be updated</param>
        /// <param name="regionStructure">New RegionStructure that will be saved</param>
        public void UpdateRegionThumbnail(Metadata metadata, RegionStructure regionStructure)
        {
            MetadataRegionCacheUpdate(metadata, regionStructure);

            string sqlCommand =
                    "UPDATE MediaPersonalRegions " +
                    "SET Thumbnail = @Thumbnail " +
                    "WHERE Broker = @Broker " +
                    "AND FileDirectory = @FileDirectory " +
                    "AND FileName = @FileName " +
                    "AND FileDateModified = @FileDateModified " +
                    "AND Type = @Type " +
                    "AND Name IS @Name " +
                    "AND Round(AreaX, " + SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals + ") = Round(@AreaX, " + SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals + ") " +
                    "AND Round(AreaY, " + SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals + ") = Round(@AreaY, " + SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals + ") " +
                    "AND Round(AreaWidth, " + SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals + ") = Round(@AreaWidth, " + SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals + ") " +
                    "AND Round(AreaHeight, " + SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals + ") = Round(@AreaHeight, " + SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals + ") " +
                    "AND RegionStructureType = @RegionStructureType";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadata.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", metadata.FileDirectory);
                commandDatabase.Parameters.AddWithValue("@FileName", metadata.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(metadata.FileDateModified));
                commandDatabase.Parameters.AddWithValue("@Type", regionStructure.Type);
                commandDatabase.Parameters.AddWithValue("@Name", regionStructure.Name);
                commandDatabase.Parameters.AddWithValue("@AreaX", regionStructure.AreaX);
                commandDatabase.Parameters.AddWithValue("@AreaY", regionStructure.AreaY);
                commandDatabase.Parameters.AddWithValue("@AreaWidth", regionStructure.AreaWidth);
                commandDatabase.Parameters.AddWithValue("@AreaHeight", regionStructure.AreaHeight);
                commandDatabase.Parameters.AddWithValue("@RegionStructureType", (int)regionStructure.RegionStructureType);

                if (regionStructure.Thumbnail == null)
                    commandDatabase.Parameters.AddWithValue("@Thumbnail", DBNull.Value);
                else commandDatabase.Parameters.AddWithValue("@Thumbnail", dbTools.ImageToByteArray(regionStructure.Thumbnail));

                commandDatabase.ExecuteNonQuery();
            }
        }
        #endregion

        #region Copy Metadata
        public void Copy(string oldDirectory, string oldFilename, string newDirectory, string newFilename)
        {
            MetadataCacheRemove(oldDirectory, oldFilename);
            dbTools.TransactionBeginBatch();

            string sqlCommand =
                "INSERT INTO MediaMetadata (" +
                    "Broker, FileDirectory, FileName, FileSize, " +
                    "FileDateCreated, FileDateModified, FileLastAccessed, FileMimeType, " +
                    "PersonalTitle, PersonalAlbum, PersonalDescription, PersonalComments, PersonalRatingPercent,PersonalAuthor, " +
                    "CameraMake, CameraModel, " +
                    "MediaDateTaken, MediaWidth, MediaHeight, MediaOrientation, MediaVideoLength, " +
                    "LocationAltitude, LocationLatitude, LocationLongitude, LocationDateTime, " +
                    "LocationName, LocationCountry, LocationCity, LocationState, RowChangedDated) " +
                 "SELECT " +
                     "Broker, @NewFileDirectory, @NewFileName, FileSize, " +
                     "FileDateCreated, FileDateModified, FileLastAccessed, FileMimeType, " +
                     "PersonalTitle, PersonalAlbum, PersonalDescription, PersonalComments, PersonalRatingPercent,PersonalAuthor, " +
                     "CameraMake, CameraModel, " +
                     "MediaDateTaken, MediaWidth, MediaHeight, MediaOrientation, MediaVideoLength, " +
                     "LocationAltitude, LocationLatitude, LocationLongitude, LocationDateTime, " +
                     "LocationName, LocationCountry, LocationCity, LocationState, RowChangedDated " +
                 "FROM MediaMetadata WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand =
                "INSERT INTO MediaPersonalKeywords (" +
                    "Broker, FileDirectory, FileName, FileDateModified, Keyword, Confidence) " +
                 "SELECT " +
                    "Broker, @NewFileDirectory, @NewFileName, FileDateModified, Keyword, Confidence " +
                    "FROM MediaPersonalKeywords WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand =
                "INSERT INTO MediaPersonalRegions (" +
                    "Broker, FileDirectory, FileName, FileDateModified, Type, Name, " +
                    "AreaX, AreaY, AreaWidth, AreaHeight, RegionStructureType, Thumbnail) " +
                "SELECT " +
                    "Broker, @NewFileDirectory, @NewFileName, FileDateModified, Type, " +
                    "Name, AreaX, AreaY, AreaWidth, AreaHeight, RegionStructureType, Thumbnail " +
                    "FROM MediaPersonalRegions WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand =
                "INSERT INTO MediaExiftoolTags (FileDirectory, FileName, FileDateModified, Region, Command, Parameter) " +
                "SELECT @NewFileDirectory, @NewFileName, FileDateModified, Region, Command, Parameter FROM " +
                "MediaExiftoolTags WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand =
                "INSERT INTO MediaExiftoolTagsWarning " +
                    "(FileDirectory, FileName, FileDateModified, OldRegion, OldCommand, OldParameter, NewRegion, NewCommand, NewParameter, Warning) " +
                "SELECT @NewFileDirectory, @NewFileName, FileDateModified, OldRegion, OldCommand, OldParameter, NewRegion, NewCommand, NewParameter, Warning FROM " +
                    "MediaExiftoolTagsWarning WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.ExecuteNonQuery();
            }
            dbTools.TransactionCommitBatch(false);

            sqlCommand =
                "INSERT INTO MediaThumbnail (FileDirectory, FileName, FileDateModified, Image) " +
                "SELECT @NewFileDirectory, @NewFileName, FileDateModified, Image FROM MediaThumbnail WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.ExecuteNonQuery();
            }
            dbTools.TransactionCommitBatch(false);
        }
        #endregion

        #region Move Metadata
        public bool Move(string oldDirectory, string oldFilename, string newDirectory, string newFilename)
        {
            bool movedOk = true;
            MetadataCacheRemove(oldDirectory, oldFilename);

            dbTools.TransactionBeginBatch();

            string sqlCommand =
                "UPDATE MediaMetadata SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                if (commandDatabase.ExecuteNonQuery() == -1) movedOk = false;                
            }

            sqlCommand =
                "UPDATE MediaPersonalKeywords SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                if (commandDatabase.ExecuteNonQuery() == -1) movedOk = false;
            }

            sqlCommand =
                "UPDATE MediaPersonalRegions SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                if (commandDatabase.ExecuteNonQuery() == -1) movedOk = false;
            }

            sqlCommand =
                "UPDATE MediaExiftoolTags SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                if (commandDatabase.ExecuteNonQuery() == -1) movedOk = false;
            }

            sqlCommand =
                "UPDATE MediaExiftoolTagsWarning SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                if (commandDatabase.ExecuteNonQuery() == -1) movedOk = false;
            }
            dbTools.TransactionCommitBatch(false);

            sqlCommand =
                "UPDATE MediaThumbnail SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                if (commandDatabase.ExecuteNonQuery() == -1) movedOk = false;
            }
            dbTools.TransactionCommitBatch(false);

            return movedOk;
        }
        #endregion

        #region Delete Directoy - Mediadata
        /// <summary>
        /// Delete all records that fits parameters
        /// </summary>
        /// <param name="broker">When (Broker & @Broker) = @Broker</param>
        /// <param name="fileDirectory">Media in this Folder</param>
        /// <param name="fileDateModified">When media file is modified, if null delete all</param>
        /// <returns></returns>
        private int DeleteDirectoryMediaMetadata(MetadataBrokerType broker, string fileDirectory, DateTime? fileDateModified = null)
        {
            int rowsAffected = 0;
            string sqlCommand = "DELETE FROM MediaMetadata WHERE " +
                            "(Broker & @Broker) = @Broker AND " +
                            "FileDirectory = @FileDirectory";

            if (fileDateModified != null) sqlCommand += " AND FileDateModified = @FileDateModified";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                if (fileDateModified != null) commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileDateModified));
                rowsAffected = commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            return rowsAffected;
        }
        #endregion

        #region Delete Directory - Media PersonalRegions
        /// <summary>
        /// Delete all records that fits parameters
        /// </summary>
        /// <param name="broker">When (Broker & @Broker) = @Broker</param>
        /// <param name="fileDirectory">Media in this Folder</param>
        /// <param name="fileDateModified">When media file is modified, if null delete all</param>
        /// <returns></returns>
        private int DeleteDirectoryMediaPersonalRegions(MetadataBrokerType broker, string fileDirectory, DateTime? fileDateModified = null)
        {
            int rowsAffected = 0;
            string sqlCommand = "DELETE FROM MediaPersonalRegions WHERE " +
                            "(Broker & @Broker) = @Broker AND " +
                            "FileDirectory = @FileDirectory";
            if (fileDateModified != null) sqlCommand += " AND FileDateModified = @FileDateModified";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                if (fileDateModified != null) commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileDateModified));
                rowsAffected = commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            return rowsAffected;
        }
        #endregion

        #region Delete Directory - Media PersonalKeywords 
        /// <summary>
        /// Delete all records that fits parameters
        /// </summary>
        /// <param name="broker">When (Broker & @Broker) = @Broker</param>
        /// <param name="fileDirectory">Media in this Folder</param>
        /// <param name="fileDateModified">When media file is modified, if null delete all</param>
        /// <returns></returns>
        private int DeleteDirectoryMediaPersonalKeywords(MetadataBrokerType broker, string fileDirectory, DateTime? fileDateModified = null)
        {
            int rowsAffected = 0; 
            string sqlCommand = "DELETE FROM MediaPersonalKeywords WHERE " +
                            "(Broker & @Broker) = @Broker AND " +
                            "FileDirectory = @FileDirectory";
            if (fileDateModified != null) sqlCommand += " AND FileDateModified = @FileDateModified";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                if (fileDateModified != null) commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileDateModified));
                rowsAffected = commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            return rowsAffected;
        }
        #endregion

        #region Delete Directory
        /// <summary>
        /// Delete all records and sub-records that fits parameters
        /// </summary>
        /// <param name="broker">When (Broker & @Broker) = @Broker</param>
        /// <param name="fileDirectory">Media in this Folder</param>
        /// <param name="fileDateModified">When media file is modified, if null delete all</param>
        /// <returns></returns>
        public int DeleteDirectoryAndHistory(MetadataBrokerType broker, string fileDirectory, DateTime? dateTime = null)
        {
            int rowsAffected = 0;
            int rowsAffectedTotal = 0;
            webScrapingPackageDates = null;
            TransactionCommitBatch(true);
            if (dateTime == null) MetadataCacheRemove(fileDirectory);
            else MetadataCacheRemove(broker, fileDirectory, (DateTime)dateTime);
            rowsAffected += DeleteDirectoryMediaMetadata(broker, fileDirectory, dateTime);
            if (rowsAffected >= 0) rowsAffectedTotal += rowsAffected;
            rowsAffected += DeleteDirectoryMediaPersonalRegions(broker, fileDirectory, dateTime);
            if (rowsAffected >= 0) rowsAffectedTotal += rowsAffected;
            rowsAffected += DeleteDirectoryMediaPersonalKeywords(broker, fileDirectory, dateTime);
            if (rowsAffected >= 0) rowsAffectedTotal += rowsAffected;
            return rowsAffectedTotal;
        }
        #endregion

        #region Delete File - Metadata
        private void DeleteFileEntryFromMediaMetadata(FileEntryBroker fileEntryBroker)
        {
            List<FileEntryBroker> fileEntrieBrokers = new List<FileEntryBroker>();
            fileEntrieBrokers.Add(fileEntryBroker);
            DeleteFileEntriesFromMediaMetadata(fileEntrieBrokers);
        }
        /// <summary>
        /// Delete all records that fits parameters
        /// </summary>
        /// <param name="fileEntryBroker">Folder, Filename and When (Broker & @Broker) = @Broker</param>
        private void DeleteFileEntriesFromMediaMetadata(List<FileEntryBroker> fileEntryBrokers)
        {
            if (fileEntryBrokers.Count() == 0) return;

            DeleteRecordEventArgs deleteRecordEventArgsInit = new DeleteRecordEventArgs();
            deleteRecordEventArgsInit.HashQueue = fileEntryBrokers.GetHashCode();
            deleteRecordEventArgsInit.TableName = "Metadata";
            deleteRecordEventArgsInit.FileEntries = fileEntryBrokers.Count();
            OnDeleteRecord(this, deleteRecordEventArgsInit);

            string sqlCommand = "DELETE FROM MediaMetadata WHERE " +
                            "(Broker & @Broker) = @Broker " +
                            "AND FileDirectory = @FileDirectory " +
                            "AND FileName = @FileName " +
                            "AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
                {
                    deleteRecordEventArgsInit.Count++;
                    DeleteRecordEventArgs deleteRecordEventArgs = new DeleteRecordEventArgs(deleteRecordEventArgsInit);
                    
                    if (OnDeleteRecord != null) OnDeleteRecord(this, deleteRecordEventArgs);

                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@Broker", fileEntryBroker.Broker);
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntryBroker.Directory);
                    commandDatabase.Parameters.AddWithValue("@FileName", fileEntryBroker.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntryBroker.LastWriteDateTime));
                    commandDatabase.ExecuteNonQuery();      // Execute the query
                }
            }

            DeleteRecordEventArgs deleteRecordEventArgsEnd = new DeleteRecordEventArgs(deleteRecordEventArgsInit);
            deleteRecordEventArgsEnd.Aborted = true;
            if (OnDeleteRecord != null) OnDeleteRecord(this, deleteRecordEventArgsEnd);
        }
        #endregion

        #region Delete File - Personal Regions
        private void DeleteFileEntryFromMediaPersonalRegions(FileEntryBroker fileEntryBroker)
        {
            List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();
            fileEntryBrokers.Add(fileEntryBroker);
            DeleteFileEntriesFromMediaPersonalRegions(fileEntryBrokers);
        }
        /// <summary>
        /// Delete all records that fits parameters
        /// </summary>
        /// <param name="fileEntryBroker">Folder, Filename and When (Broker & @Broker) = @Broker</param>
        private void DeleteFileEntriesFromMediaPersonalRegions(List<FileEntryBroker> fileEntryBrokers)
        {
            DeleteRecordEventArgs deleteRecordEventArgs = new DeleteRecordEventArgs();
            deleteRecordEventArgs.HashQueue = fileEntryBrokers.GetHashCode();
            deleteRecordEventArgs.TableName = "Regions";
            deleteRecordEventArgs.FileEntries = fileEntryBrokers.Count();
            if (deleteRecordEventArgs.FileEntries == 0)
            {
                OnDeleteRecord(this, deleteRecordEventArgs);
                return;
            }

            string sqlCommand = "DELETE FROM MediaPersonalRegions WHERE " +
                "(Broker & @Broker) = @Broker " +
                "AND FileDirectory = @FileDirectory " +
                "AND FileName = @FileName " +
                "AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
                {
                    deleteRecordEventArgs.Count++;
                    if (OnDeleteRecord != null) OnDeleteRecord(this, deleteRecordEventArgs);

                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@Broker", fileEntryBroker.Broker);
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntryBroker.Directory);
                    commandDatabase.Parameters.AddWithValue("@FileName", fileEntryBroker.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntryBroker.LastWriteDateTime));
                    commandDatabase.ExecuteNonQuery();      // Execute the query
                }
            }
        }
        #endregion

        #region Delete File - Personal Keywords
        private void DeleteFileEntryFromMediaPersonalKeywords(FileEntryBroker fileEntryBroker)
        {
            List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();
            fileEntryBrokers.Add(fileEntryBroker);
            DeleteFileEntriesFromMediaPersonalKeywords(fileEntryBrokers);
        }

        /// <summary>
        /// Delete all records that fits parameters
        /// </summary>
        /// <param name="fileEntryBroker">Folder, Filename and When (Broker & @Broker) = @Broker</param>
        private void DeleteFileEntriesFromMediaPersonalKeywords(List<FileEntryBroker> fileEntryBrokers)
        {
            DeleteRecordEventArgs deleteRecordEventArgs = new DeleteRecordEventArgs();
            deleteRecordEventArgs.HashQueue = fileEntryBrokers.GetHashCode();
            deleteRecordEventArgs.TableName = "Keywords";
            deleteRecordEventArgs.FileEntries = fileEntryBrokers.Count();
            if (deleteRecordEventArgs.FileEntries == 0)
            {
                OnDeleteRecord(this, deleteRecordEventArgs);
                return;
            }

            string sqlCommand = "DELETE FROM MediaPersonalKeywords WHERE " +
                            "(Broker & @Broker) = @Broker " +
                            "AND FileDirectory = @FileDirectory " +
                            "AND FileName = @FileName " +
                            "AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
                {
                    deleteRecordEventArgs.Count++;
                    if (OnDeleteRecord != null) OnDeleteRecord(this, deleteRecordEventArgs);

                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@Broker", fileEntryBroker.Broker);
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntryBroker.Directory);
                    commandDatabase.Parameters.AddWithValue("@FileName", fileEntryBroker.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntryBroker.LastWriteDateTime));
                    commandDatabase.ExecuteNonQuery();      // Execute the query
                }
            }
        }
        #endregion

        #region Delete FileEntries
        public void DeleteFileEntry(FileEntryBroker fileEntryBroker)
        {
            List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();
            fileEntryBrokers.Add(fileEntryBroker);
            DeleteFileEntries(fileEntryBrokers);
        }
         
        public void DeleteFileEntries(List<FileEntryBroker> fileEntryBrokers)
        {
            DeleteFileEntriesFromMediaMetadata(fileEntryBrokers);
            DeleteFileEntriesFromMediaPersonalRegions(fileEntryBrokers);
            DeleteFileEntriesFromMediaPersonalKeywords(fileEntryBrokers);
            MetadataCacheRemove(fileEntryBrokers);
        }
        #endregion

        

        #region WebScraping
        public const string WebScapingFolderName = "WebScraper";
        private static List<DateTime> webScrapingPackageDates = null;

        #region WebScraping - Write
        public void WebScrapingWrite(Metadata metadata)
        {
            webScrapingPackageDates = null;
            Write(metadata);
        }
        #endregion 

        #region WebScraping - ListWebScraperPackages
        public List<DateTime> ListWebScraperDataSet(MetadataBrokerType broker, string directory)
        {
            if (webScrapingPackageDates != null) return webScrapingPackageDates;

            List<DateTime> webScrapingPackages = new List<DateTime>();

            string sqlCommand = @"SELECT DISTINCT FileDateModified FROM MediaMetadata 
                    WHERE (Broker & @Broker) = @Broker AND FileDirectory = @FileDirectory ORDER BY FileDateModified DESC";
                
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", directory);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        webScrapingPackages.Add((DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]));
                    }
                }
            }

            webScrapingPackageDates = webScrapingPackages;
            return webScrapingPackages;
        }
        #endregion 

        #region WebScraping - ListMediafilesInWebScraperPackages
        public List<FileEntryBroker> ListMediafilesInWebScraperPackages(MetadataBrokerType broker, string directory, DateTime fileDateModified)
        {
            List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();

            string sqlCommand = @"SELECT Broker, FileDirectory, FileName, FileDateModified FROM MediaMetadata 
                WHERE (Broker & @Broker) = @Broker
                AND FileDirectory = @FileDirectory 
                AND FileDateModified = @FileDateModified";
            
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", directory);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileDateModified));
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FileEntryBroker fileEntryBroker = new FileEntryBroker
                            (
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]),
                            (MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"])
                            );
                        fileEntryBrokers.Add(fileEntryBroker);
                    }
                }
            }

            return fileEntryBrokers;
        }
        #endregion

        #region WebScraping - GetWebScraperLastPackageDate
        public DateTime? GetWebScraperLastPackageDate()
        {
            DateTime? dateTimeResult = null;
            List<DateTime> webScrapingPackageDates = ListWebScraperDataSet(MetadataBrokerType.WebScraping, WebScapingFolderName);
            if (webScrapingPackageDates.Count > 0) dateTimeResult = webScrapingPackageDates[0];
            foreach (DateTime dateTimeCheck in webScrapingPackageDates) if (dateTimeCheck > dateTimeResult) dateTimeResult = dateTimeCheck;
            return (dateTimeResult == null ? DateTime.MinValue : dateTimeResult);
        }
        #endregion

        #region WebScraping - ReadWebScraperMetadataFromCacheOrDatabase
        public Metadata ReadWebScraperMetadataFromCacheOrDatabase(FileEntryBroker fileEntryBroker)
        {
            DateTime? dateTime = GetWebScraperLastPackageDate();
            if (dateTime == null) return null;
            FileEntryBroker fileEntryBrokerWebScraperSearch = new FileEntryBroker(WebScapingFolderName, fileEntryBroker.FileName, (DateTime)dateTime, fileEntryBroker.Broker);
            return ReadMetadataFromCacheOrDatabase(fileEntryBrokerWebScraperSearch);
        }
        #endregion 

        #endregion

        #region List File Date Versions

        #region List File Date Versions - Broker
        public List<FileEntryBroker> ListFileEntryBrokerDateVersions(MetadataBrokerType broker, string fullFileName)
        {
            List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();

            string sqlCommand =
                "SELECT " +
                    "Broker, FileDirectory, FileName, FileDateModified " +
                    "FROM MediaMetadata WHERE " +
                    "(Broker & @Broker) = @Broker AND " +
                    "FileDirectory = @FileDirectory AND " +
                    "FileName = @FileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)broker);
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fullFileName));
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fullFileName));
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FileEntryBroker fileEntryBroker = new FileEntryBroker
                            (
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]),
                            (MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"])
                            );
                        fileEntryBrokers.Add(fileEntryBroker);
                    }
                }
            }

            return fileEntryBrokers;
        }
        #endregion 

        private static Dictionary<FileBroker, List<FileEntryAttribute>> listFileAttributeDateVersions = new Dictionary<FileBroker, List<FileEntryAttribute>>();
        private static readonly Object _listFileAttributeDateVersionsLock = new Object();

        #region List File Date Versions - Attribute
        /// <summary>
        /// List all versions for a media file, Broker, Folder, Filename, Modified
        /// </summary>
        /// <param name="broker">Also read (Broker & @Broker) = @Broker) to get ErrorVersions also</param>
        /// <param name="fullFileName">Filename</param>
        /// <returns></returns>
        public List<FileEntryAttribute> ListFileEntryAttributesCache(MetadataBrokerType broker, string fullFileName)
        {
            List<FileEntryAttribute> fileEntryAttributes = new List<FileEntryAttribute>();
            try
            {
                FileBroker fileBroker = new FileBroker(broker, fullFileName);
                lock (_listFileAttributeDateVersionsLock)
                {
                    if (listFileAttributeDateVersions.ContainsKey(fileBroker)) return listFileAttributeDateVersions[fileBroker];
                }

                ListFileEntryAttributes2(ref fileEntryAttributes, broker, fullFileName);
                MetadataBrokerType broker2 = broker | MetadataBrokerType.ExifToolWriteError;
                ListFileEntryAttributes2(ref fileEntryAttributes, broker2, fullFileName);

                lock (_listFileAttributeDateVersionsLock)
                {
                    listFileAttributeDateVersions.Add(fileBroker, fileEntryAttributes);
                }
                
            } catch (Exception ex)
            {
                Logger.Error(ex, "ListFileEntryAttributesCache");
            }

            return fileEntryAttributes;
        }

        /// <summary>
        /// List all versions for a media file, Broker, Folder, Filename, Modified
        /// </summary>
        /// <param name="FileEntryAttributes">List to updated, wgen when Read ExifTool first, then ExiftooError after</param>
        /// <param name="broker">Use excact Broker type (Don't add | (Broker & @Broker) = @Broker)</param>
        /// <param name="fullFileName">Filename</param>
        private void ListFileEntryAttributes2(ref List<FileEntryAttribute> FileEntryAttributes, MetadataBrokerType broker, string fullFileName)
        {
            
            string sqlCommand =
                "SELECT " +
                    "Broker, FileDirectory, FileName, FileDateModified " +
                    "FROM MediaMetadata WHERE " +
                    "Broker = @Broker AND " + //(Broker & @Broker) = @Broker
                    "FileDirectory = @FileDirectory AND " +
                    "FileName = @FileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)broker);
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fullFileName));
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fullFileName));
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    FileEntryAttribute newstFileEntryAttributeForEdit = null;

                    while (reader.Read())
                    {
                        bool isErrorVersion = false;
                        DateTime currentMetadataDate = (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]);
                        if (((MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"]) & MetadataBrokerType.ExifToolWriteError) == MetadataBrokerType.ExifToolWriteError) 
                            isErrorVersion = true;
                        FileEntryAttribute fileEntryAttribute = new FileEntryAttribute
                            (
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            currentMetadataDate,
                            isErrorVersion ? FileEntryVersion.Error : FileEntryVersion.Historical
                            );
                        FileEntryAttributes.Add(fileEntryAttribute);

                        if (!isErrorVersion && (newstFileEntryAttributeForEdit == null || currentMetadataDate > newstFileEntryAttributeForEdit.LastWriteDateTime))
                        {
                            newstFileEntryAttributeForEdit = new FileEntryAttribute((FileEntry)fileEntryAttribute, FileEntryVersion.Current);
                        }
                    }
                    if (newstFileEntryAttributeForEdit != null) FileEntryAttributes.Add(newstFileEntryAttributeForEdit);                    
                }
            }
            //return FileEntryAttributes;
        }
        #endregion 

        #endregion

        #region List Files - Missing Entries
        /// <summary>
        /// 
        /// </summary>
        /// <param name="broker"></param>
        /// <param name="files"></param>
        /// <returns>List all files not in Database, When StopCaching is set, process stops, due to user cancelled.</returns>
        public List<FileEntry> ListAllMissingFileEntries(MetadataBrokerType broker, List<FileEntry> files)
        {
            if (files == null) return null;

            List<FileEntry> mediaFilesNoInDatabase = new List<FileEntry>();

            ReadToCache(files, broker); // Faster read

            foreach (FileEntry file in files)
            {
                FileEntryBroker fileEntryBroker = new FileEntryBroker(file.FileFullPath, file.LastWriteDateTime, broker);
                Metadata metadata = ReadMetadataFromCacheOnly(fileEntryBroker);
                if (metadata == null) mediaFilesNoInDatabase.Add(new FileEntry(fileEntryBroker));
            }
            
            return mediaFilesNoInDatabase;
        }
        #endregion

        #region List files - Search
        /// <summary>
        /// Uses for search for media data using a huge list of parameres THAT IS selected to use
        /// </summary>
        /// <param name="broker">Don't find Error version only excat verson (NB, Don't have (Broker & @Broker) = @Broker) )</param>
        /// <param name="useAndBetweenGrups">When true, use AND else OR between search groups</param>
        /// <param name="useMediaTakenFrom">When true, add parameter mediaTakenFrom</param>
        /// <param name="mediaTakenFrom">When media takes is equal or newer than this date</param>
        /// <param name="useMediaTakenTo">When true, add parameter mediaTakenTo</param>
        /// <param name="mediaTakenTo">When media takes is equal or older than this date</param>
        /// <param name="isMediaTakenNull">When media takes is null</param>
        /// <param name="useAndBetweenTextTags">When true, use AND else OR between Keywords tags</param>
        /// <param name="usePersonalAlbum">When true, add parameter personalAlbum</param>
        /// <param name="personalAlbum">Equal parameter equal to Album text</param>
        /// <param name="usePersonalTitle">When true, add parameter usePersonalComments</param>
        /// <param name="personalTitle">Equal parameter equal to Title text</param>
        /// <param name="usePersonalComments">When true, add parameter personalComments</param>
        /// <param name="personalComments">Equal parameter equal to Comments text</param>
        /// <param name="usePersonalDescription">When true, add parameter personalDescription</param>
        /// <param name="personalDescription">Equal parameter equal to Description text</param>
        /// <param name="isRatingNull">When not rating given</param>
        /// <param name="hasRating0">When rating is 0 stars</param>
        /// <param name="hasRating1">When rating is 1 star</param>
        /// <param name="hasRating2">When rating is 2 stars</param>
        /// <param name="hasRating3">When rating is 3 stars</param>
        /// <param name="hasRating4">When rating is 4 stars</param>
        /// <param name="hasRating5">When rating is 5 stars</param>
        /// <param name="useLocationName">When true, add parameterlocationName </param>
        /// <param name="locationName">Equal parameter equal to Location text</param>
        /// <param name="useLocationCity">When true, add parameter useLocationCity</param>
        /// <param name="locationCity">Equal parameter equal to City text</param>
        /// <param name="useLocationState">When true, add parameter locationState</param>
        /// <param name="locationState">Equal parameter equal to State text</param>
        /// <param name="useLocationCountry">When true, add parameter locationCountry</param>
        /// <param name="locationCountry">Equal parameter equal to Country text</param>
        /// <param name="useRegionNameList">When true, add parameter regionNameList</param>
        /// <param name="needAlRegionNames">When true, use AND else OR between Region names</param>
        /// <param name="regionNameList">List of region names to check</param>
        /// <param name="withoutRegions">When regions names doesn't exist</param>
        /// <param name="useKeywordList">When true, add parameter needAllKeywords</param>
        /// <param name="needAllKeywords">When true, use AND else OR between keywords</param>
        /// <param name="keywords">List of keyword tags to check</param>
        /// <param name="withoutKeywords">When keywords tags doesn't exists</param>
        /// <param name="checkIfHasExifWarning">When true, check if has warning</param>
        /// <param name="maxRowsInResult">Maximum rows in result set</param>
        /// <returns></returns>
        public HashSet<FileEntry> ListAllSearch(MetadataBrokerType broker, bool useAndBetweenGrups,
            bool useMediaTakenFrom, DateTime mediaTakenFrom, bool useMediaTakenTo, DateTime mediaTakenTo, bool isMediaTakenNull,
            bool useAndBetweenTextTags,
            bool usePersonalAlbum, string personalAlbum,
            bool usePersonalTitle, string personalTitle,
            bool usePersonalComments, string personalComments,
            bool usePersonalDescription, string personalDescription,
            bool isRatingNull, bool hasRating0, bool hasRating1, bool hasRating2, bool hasRating3, bool hasRating4, bool hasRating5,
            bool useLocationName, string locationName,
            bool useLocationCity, string locationCity,
            bool useLocationState, string locationState,
            bool useLocationCountry, string locationCountry,
            bool useRegionNameList, bool needAlRegionNames, List<string> regionNameList, bool withoutRegions,
            bool useKeywordList, bool needAllKeywords, List<string> keywords, bool withoutKeywords,
            bool checkIfHasExifWarning, int maxRowsInResult
            )
        {

            HashSet<FileEntry> listing = new HashSet<FileEntry>();

            string sqlCommandBasicSelect = "SELECT DISTINCT MediaMetadata.Broker, MediaMetadata.FileDirectory, MediaMetadata.FileName, MediaMetadata.FileDateModified FROM MediaMetadata ";

            #region Warning
            if (checkIfHasExifWarning) sqlCommandBasicSelect +=
                "LEFT JOIN MediaExiftoolTagsWarning ON " +
                "MediaExiftoolTagsWarning.FileDirectory = MediaMetadata.FileDirectory AND " +
                "MediaExiftoolTagsWarning.FileName = MediaMetadata.FileName AND " +
                "MediaExiftoolTagsWarning.FileDateModified = MediaMetadata.FileDateModified ";
            #endregion

            #region Get only newest records, not historical
            sqlCommandBasicSelect += "WHERE MediaMetadata.Broker = @Broker AND MediaMetadata.FileDateModified = (SELECT MAX(MediaMetadataNewst.FileDateModified) FROM MediaMetadata AS MediaMetadataNewst " +
                "WHERE MediaMetadataNewst.Broker = MediaMetadata.Broker AND MediaMetadataNewst.FileDirectory = MediaMetadata.FileDirectory AND MediaMetadataNewst.FileName = MediaMetadata.FileName) ";
            #endregion

            #region DateTaken
            if (useMediaTakenFrom && useMediaTakenTo)
                sqlCommandBasicSelect += (useAndBetweenGrups ? "AND " : "OR ") + "((MediaDateTaken >= @MediaDateTakenFrom AND MediaDateTaken <= @MediaDateTakenTo) " + (isMediaTakenNull ? "OR MediaDateTaken IS NULL " : "") + ") ";
            else if (useMediaTakenFrom) sqlCommandBasicSelect += (useAndBetweenGrups ? "AND " : "OR ") + "(MediaDateTaken >= @MediaDateTakenFrom " + (isMediaTakenNull ? "OR MediaDateTaken IS NULL " : "") + ") ";
            else if (useMediaTakenTo) sqlCommandBasicSelect += (useAndBetweenGrups ? "AND " : "OR ") + "(MediaDateTaken <= @MediaDateTakenTo " + (isMediaTakenNull ? "OR MediaDateTaken IS NULL " : "") + ") ";
            else if (isMediaTakenNull) sqlCommandBasicSelect += (useAndBetweenGrups ? "AND " : "OR ") + "MediaDateTaken IS NULL ";
            #endregion

            string sqlCommand = "";

            #region Text field tags
            string sqlTextTags = "";
            if (usePersonalAlbum) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (personalAlbum == null ? "PersonalAlbum IS NULL " : "PersonalAlbum LIKE @PersonalAlbum ");
            if (usePersonalTitle) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (personalTitle == null ? "PersonalTitle IS NULL " : "PersonalTitle LIKE @PersonalTitle ");
            if (usePersonalComments) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (personalComments == null ? "PersonalComments IS NULL " : "PersonalComments LIKE @PersonalComments ");
            if (usePersonalDescription) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (personalDescription == null ? "PersonalDescription IS NULL " : "PersonalDescription LIKE @PersonalDescription ");
            if (useLocationName) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (locationName == null ? "LocationName IS NULL " : "LocationName LIKE @LocationName ");
            if (useLocationCity) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (locationCity == null ? "LocationCity IS NULL " : "LocationCity LIKE @LocationCity ");
            if (useLocationState) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (locationState == null ? "LocationState IS NULL " : "LocationState LIKE @LocationState ");
            if (useLocationCountry) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (locationCountry == null ? "LocationCountry IS NULL " : "LocationCountry LIKE @LocationCountry ");
            if (sqlTextTags != "") sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + "(" + sqlTextTags + ") ";
            #endregion

            #region Rating
            string sqlRating = "";
            if (isRatingNull) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent IS NULL)";
            if (hasRating0) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent < 1)";
            if (hasRating1) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >=  1 AND PersonalRatingPercent <= 12)";
            if (hasRating2) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >  12 AND PersonalRatingPercent <= 37)";
            if (hasRating3) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >  37 AND PersonalRatingPercent <= 62)";
            if (hasRating4) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >  62 AND PersonalRatingPercent <= 87)";
            if (hasRating5) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >  87)";
            if (sqlRating != "") sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + "(" + sqlRating + ") ";
            #endregion

            #region Warning 
            if (checkIfHasExifWarning) sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + "(MediaExiftoolTagsWarning.Warning IS NOT NULL) ";
            #endregion

            #region Region Names
            if ((useRegionNameList && regionNameList != null && regionNameList.Count > 0) || withoutRegions)
            {
                string sqlRegionsNotInList = "";
                if (withoutRegions)
                {
                    sqlRegionsNotInList =
                        "(SELECT 1 FROM MediaPersonalRegions WHERE MediaPersonalRegions.Broker = MediaMetadata.Broker AND " +
                        "MediaPersonalRegions.FileDirectory = MediaMetadata.FileDirectory AND " +
                        "MediaPersonalRegions.FileDateModified = MediaMetadata.FileDateModified AND " +
                        "MediaPersonalRegions.FileName = MediaMetadata.FileName) IS NULL ";
                }

                string sqlRegionNames = "";
                if ((useRegionNameList && regionNameList != null && regionNameList.Count > 0) || withoutRegions)
                {
                    for (int index = 0; index < regionNameList.Count; index++)
                    {
                        sqlRegionNames += (sqlRegionNames == "" ? "" : needAlRegionNames ? " AND " : " OR ") +
                            "(SELECT 1 FROM MediaPersonalRegions WHERE MediaPersonalRegions.Broker = MediaMetadata.Broker AND " +
                            "MediaPersonalRegions.FileDirectory = MediaMetadata.FileDirectory AND " +
                            "MediaPersonalRegions.FileName = MediaMetadata.FileName AND " +
                            "MediaPersonalRegions.FileDateModified = MediaMetadata.FileDateModified AND " +
                            "MediaPersonalRegions.Name " + (regionNameList[index] == null ? "IS NULL) = 1" : "LIKE @MediaPersonalRegionsName" + index.ToString() + ") = 1");
                    }
                }
                sqlRegionNames =
                    (sqlRegionsNotInList == "" ? "" : sqlRegionsNotInList) +
                    (sqlRegionsNotInList != "" && sqlRegionNames != "" ? " OR " : "") +
                    (sqlRegionNames == "" ? "" : " (" + sqlRegionNames + ")");

                sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + "(" + sqlRegionNames + ") ";
            }
            #endregion

            #region Keywords
            if ((useKeywordList && keywords != null && keywords.Count > 0) || withoutKeywords)
            {

                string sqlKeywordNotInList = "";
                if (withoutKeywords)
                {
                    sqlKeywordNotInList =
                        "(SELECT 1 FROM MediaPersonalKeywords WHERE MediaPersonalKeywords.Broker = MediaMetadata.Broker AND " +
                        "MediaPersonalKeywords.FileDirectory = MediaMetadata.FileDirectory AND " +
                        "MediaPersonalKeywords.FileDateModified = MediaMetadata.FileDateModified AND " +
                        "MediaPersonalKeywords.FileName = MediaMetadata.FileName) IS NULL ";
                }

                string sqlKeywordList = "";
                if (useKeywordList && keywords != null && keywords.Count > 0)
                {
                    for (int index = 0; index < keywords.Count; index++)
                    {
                        sqlKeywordList += (sqlKeywordList == "" ? "" : needAllKeywords ? " AND " : " OR ") +
                             "(SELECT 1 FROM MediaPersonalKeywords WHERE MediaPersonalKeywords.Broker = MediaMetadata.Broker AND " +
                             "MediaPersonalKeywords.FileDirectory = MediaMetadata.FileDirectory AND " +
                             "MediaPersonalKeywords.FileName = MediaMetadata.FileName AND " +
                             "MediaPersonalKeywords.FileDateModified = MediaMetadata.FileDateModified AND " +
                             "MediaPersonalKeywords.Keyword " + (keywords[index] == null ? "IS NULL) = 1" : "LIKE @MediaPersonalKeywordsKeyword" + index.ToString() + ") = 1");
                    }
                }

                sqlKeywordList =
                    (sqlKeywordNotInList == "" ? "" : sqlKeywordNotInList) +
                    (sqlKeywordNotInList != "" && sqlKeywordList != "" ? " OR " : "") +
                    (sqlKeywordList == "" ? "" : " (" + sqlKeywordList + ")");

                sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + " (" + sqlKeywordList + ") ";
            }
            #endregion

            sqlCommand = sqlCommandBasicSelect + (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND (" + sqlCommand + ") " : "OR (" + sqlCommand + ")");
            //sqlCommand = sqlCommandBasicSelect + (sqlCommand == "" ? "" : "AND (" + sqlCommand + ") ");  

            #region Limit
            sqlCommand += "LIMIT " + maxRowsInResult.ToString();
            #endregion 

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)broker);

                #region DateTaken
                if (useMediaTakenFrom) commandDatabase.Parameters.AddWithValue("@MediaDateTakenFrom", dbTools.ConvertFromDateTimeToDBVal(mediaTakenFrom));
                if (useMediaTakenTo) commandDatabase.Parameters.AddWithValue("@MediaDateTakenTo", dbTools.ConvertFromDateTimeToDBVal(mediaTakenTo));
                #endregion

                #region Text field tags
                if (usePersonalAlbum) commandDatabase.Parameters.AddWithValue("@PersonalAlbum", personalAlbum);
                if (usePersonalTitle) commandDatabase.Parameters.AddWithValue("@PersonalTitle", personalTitle);
                if (usePersonalComments) commandDatabase.Parameters.AddWithValue("@PersonalComments", personalComments);
                if (usePersonalDescription) commandDatabase.Parameters.AddWithValue("@PersonalDescription", personalDescription);
                if (useLocationName) commandDatabase.Parameters.AddWithValue("@LocationName", locationName);
                if (useLocationCity) commandDatabase.Parameters.AddWithValue("@LocationCity", locationCity);
                if (useLocationState) commandDatabase.Parameters.AddWithValue("@LocationState", locationState);
                if (useLocationCountry) commandDatabase.Parameters.AddWithValue("@LocationCountry", locationCountry);
                #endregion

                #region Region names
                if (useRegionNameList && regionNameList != null && regionNameList.Count > 0)
                {
                    for (int index = 0; index < regionNameList.Count; index++) commandDatabase.Parameters.AddWithValue("@MediaPersonalRegionsName" + index.ToString(), regionNameList[index]);
                }
                #endregion

                #region Keywords
                if (useKeywordList && keywords != null && keywords.Count > 0)
                {
                    for (int index = 0; index < keywords.Count; index++) commandDatabase.Parameters.AddWithValue("@MediaPersonalKeywordsKeyword" + index.ToString(), keywords[index]);
                }
                #endregion

                #region Read list of file fullpaths
                //commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listing.Add(new FileEntry(
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"])
                            ));
                    }
                }
                #endregion 
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalAlbums()
        public List<string> ListAllPersonalAlbums(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalAlbum FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalAlbum"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalDescriptions()
        public List<string> ListAllPersonalDescriptions(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalDescription FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalDescription"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalTitles()
        public List<string> ListAllPersonalTitles(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalTitle FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalTitle"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalComments()
        public List<string> ListAllPersonalComments(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalComments FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalComments"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalAuthors()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadataBrokerType"></param>
        /// <returns></returns>
        public List<string> ListAllPersonalAuthors(MetadataBrokerType metadataBrokerType)
        {

            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalAuthor FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalAuthor"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllLocationNames()
        public List<string> ListAllLocationNames(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT LocationName FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["LocationName"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllLocationCities()
        public List<string> ListAllLocationCities(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT LocationCity FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["LocationCity"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllLocationStates()
        public List<string> ListAllLocationStates(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT LocationState FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["LocationState"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllLocationCountries()
        public List<string> ListAllLocationCountries(MetadataBrokerType metadataBrokerType)
        {

            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT LocationCountry FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["LocationCountry"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalRegionNameCountCache
        private static Dictionary<MetadataBrokerType, Dictionary<StringNullable, int>> metadataRegionNameCountCache = null;
        private static readonly Object metadataRegionNameCountCacheLock = new Object();

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameCountCacheClear
        public void ListAllPersonalRegionNameCountCacheClear()
        {
            lock (metadataRegionNameCountCacheLock)
            {
                metadataRegionNameCountCache = null;
                metadataRegionNameCountCache = new Dictionary<MetadataBrokerType, Dictionary<StringNullable, int>>();
            }
        }
        #endregion

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameCount
        private Dictionary<StringNullable, int> ListAllPersonalRegionNameCount(MetadataBrokerType metadataBrokerType)
        {
            //Private due to fake null hack
            Dictionary<StringNullable, int> metadataRegionNameCountDictionary = new Dictionary<StringNullable, int>();

            string sqlCommand =
                "SELECT Name, Count(1) AS CountNames FROM MediaPersonalRegions WHERE Broker = @Broker GROUP BY Name";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = dbTools.ConvertFromDBValString(reader["Name"]);
                        metadataRegionNameCountDictionary.Add(new StringNullable(name), (int)dbTools.ConvertFromDBValInt(reader["CountNames"]));
                    }
                }
            }
            return metadataRegionNameCountDictionary;
        }
        #endregion

        #region RegionNamesUpdated
        public void PersonalRegionNameCountCacheUpdated(MetadataBrokerType metadataBrokerType, string name)
        {
            if (metadataRegionNameCountCache == null) 
                metadataRegionNameCountCache = new Dictionary<MetadataBrokerType, Dictionary<StringNullable, int>>(); //It should already been created, why isn'y
            if (!metadataRegionNameCountCache.ContainsKey(metadataBrokerType)) 
                metadataRegionNameCountCache.Add(metadataBrokerType, new Dictionary<StringNullable, int>()); //It should already been created, why isn'y
            StringNullable stringNullableName = new StringNullable(name);
            if (!metadataRegionNameCountCache[metadataBrokerType].ContainsKey(stringNullableName)) 
            {
                metadataRegionNameCountCache[metadataBrokerType].Add(stringNullableName, 1);
            } else
            {
                metadataRegionNameCountCache[metadataBrokerType][stringNullableName]++;
            }
        }

        #endregion 

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameCountCache
        private Dictionary<StringNullable, int> ListAllPersonalRegionNameCountCache(MetadataBrokerType metadataBrokerType)
        {
            lock (metadataRegionNameCountCacheLock)
            {
                if (metadataRegionNameCountCache == null) metadataRegionNameCountCache = new Dictionary<MetadataBrokerType, Dictionary<StringNullable, int>>();
                if (!metadataRegionNameCountCache.ContainsKey(metadataBrokerType)) metadataRegionNameCountCache.Add(metadataBrokerType, new Dictionary<StringNullable, int>());
                metadataRegionNameCountCache[metadataBrokerType] = ListAllPersonalRegionNameCount(metadataBrokerType);
                return metadataRegionNameCountCache[metadataBrokerType];
            }
        }
        #endregion

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameCountCache
        public List<string> ListAllPersonalRegionNameNotInListCache(MetadataBrokerType metadataBrokerType, List<string> namesdontIncludeList1, List<string> namesdontIncludeList2, int topCount, bool includeEmpty = false)
        {
            List<string> list = new List<string>();
            Dictionary<StringNullable, int> metadataRegionNameCountDictionary = ListAllPersonalRegionNameCountCache(metadataBrokerType);

            var ordered = metadataRegionNameCountDictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            int count = 0;
            if (count < topCount)
            {
                foreach (KeyValuePair<StringNullable, int> keyValuePair in ordered)
                {
                    bool doNotInclude = false;
                    StringNullable brokerRegionName = keyValuePair.Key;
                    if (namesdontIncludeList1 != null && namesdontIncludeList1.Contains(keyValuePair.Key.StringValue)) doNotInclude = true;
                    if (namesdontIncludeList2 != null && namesdontIncludeList2.Contains(keyValuePair.Key.StringValue)) doNotInclude = true;
                    if (!includeEmpty && string.IsNullOrEmpty(brokerRegionName.StringValue)) doNotInclude = true;
                    
                    if (!doNotInclude)
                    {
                        list.Add(keyValuePair.Key.StringValue);
                        count++;
                        if (count >= topCount) break;
                    }
                }
            }
            list.Sort();
            return list;
        }
        #endregion

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameTopCountCache
        public List<string> ListAllPersonalRegionNameTopCountCache(MetadataBrokerType metadataBrokerType, int topCount)
        {
            return ListAllPersonalRegionNameNotInListCache(metadataBrokerType, null, null, topCount);
        }
        #endregion

        #region ListAllPersonalRegionsCache
        public List<string> ListAllPersonalRegionsCache(MetadataBrokerType metadataBrokerType)
        {
            return ListAllPersonalRegionNameTopCountCache(metadataBrokerType, int.MaxValue); 
        }

        public List<string> ListAllPersonalRegionsCache()
        {
            List<string> joinAllRegions = ListAllPersonalRegionNameTopCountCache(MetadataBrokerType.ExifTool, int.MaxValue);

            List<string> joinAddRegions = ListAllPersonalRegionNameTopCountCache(MetadataBrokerType.WebScraping, int.MaxValue);
            foreach (string addRegion in joinAddRegions) if (!joinAllRegions.Contains(addRegion)) joinAllRegions.Add(addRegion);

            joinAddRegions = ListAllPersonalRegionNameTopCountCache(MetadataBrokerType.WindowsLivePhotoGallery, int.MaxValue);
            foreach (string addRegion in joinAddRegions) if (!joinAllRegions.Contains(addRegion)) joinAllRegions.Add(addRegion);

            joinAddRegions = ListAllPersonalRegionNameTopCountCache(MetadataBrokerType.MicrosoftPhotos, int.MaxValue);
            foreach (string addRegion in joinAddRegions) if (!joinAllRegions.Contains(addRegion)) joinAllRegions.Add(addRegion);

            joinAllRegions.Sort();
            return joinAllRegions;
        }
        #endregion

        #endregion

        #region MetadataRegionNamesCache
        private static Dictionary<MetadataRegionNameKey, List<string>> metadataRegionNamesCache = new Dictionary<MetadataRegionNameKey, List<string>>();
        private static readonly Object _metadataRegionNamesCacheLock = new Object();

        #region MetadataRegionNamesCache - MetadataRegionNamesCacheClear
        public void MetadataRegionNamesCacheClear()
        {
            lock (_metadataRegionNamesCacheLock)
            {
                metadataRegionNamesCache = null;
                metadataRegionNamesCache = new Dictionary<MetadataRegionNameKey, List<string>>();
            }
        }
        #endregion

        #region MetadataRegionNamesCache - ListAllRegionNamesCache
        public List<string> ListAllRegionNamesCache(MetadataBrokerType metadataBrokerType, DateTime? dateTimeFrom, DateTime? dateTimeTo)
        {
            MetadataRegionNameKey metadataRegionNameKey = new MetadataRegionNameKey(metadataBrokerType, 
                (dateTimeFrom == null ? DateTime.MinValue : (DateTime)dateTimeFrom), 
                (dateTimeTo == null ? DateTime.MaxValue : (DateTime)dateTimeTo)
                );
            
            if (MetadataRegionNamesCacheContainsKey(metadataRegionNameKey)) return MetadataRegionNamesCacheGet(metadataRegionNameKey);

            List<string> regionNames = ListAllRegionNames(metadataBrokerType, dateTimeFrom, dateTimeTo);
            if (regionNames != null)
            {
                MetadataRegionNamesCacheUpdate(metadataRegionNameKey, regionNames);
                return regionNames;
            }
            else return null;            
        }
        #endregion

        #region MetadataRegionNamesCache - ListAllRegionNames
        public List<string> ListAllRegionNames(MetadataBrokerType metadataBrokerType, DateTime? dateTimeFrom, DateTime? dateTimeTo)
        {
            List<string> listing = new List<string>();

            string sqlCommand = "";
            if (metadataBrokerType != MetadataBrokerType.Empty) sqlCommand += (string.IsNullOrEmpty(sqlCommand) ? "" : "AND ") + "Broker = @Broker ";
            if (dateTimeFrom != null) sqlCommand += (string.IsNullOrEmpty(sqlCommand) ? "" : "AND ") + "FileDateModified >= @FileDateModifiedFrom ";
            if (dateTimeTo != null) sqlCommand += (string.IsNullOrEmpty(sqlCommand) ? "" : "AND ") + "FileDateModified <= @FileDateModifiedTo ";

            sqlCommand = "SELECT DISTINCT Name FROM MediaPersonalRegions " + (string.IsNullOrEmpty(sqlCommand) ? "" : "WHERE ") + sqlCommand;
            
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                if (metadataBrokerType != MetadataBrokerType.Empty) commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                if (dateTimeFrom != null) commandDatabase.Parameters.AddWithValue("@FileDateModifiedFrom", dbTools.ConvertFromDateTimeToDBVal(dateTimeFrom));
                if (dateTimeTo != null) commandDatabase.Parameters.AddWithValue("@FileDateModifiedTo", dbTools.ConvertFromDateTimeToDBVal(dateTimeTo));

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["Name"]));
                }
            }

            return listing;
        }
        #endregion

        #region MetadataRegionNamesCache - MetadataRegionNamesCacheContainsKey
        private bool MetadataRegionNamesCacheContainsKey(MetadataRegionNameKey key)
        {
            lock (_metadataRegionNamesCacheLock) return metadataRegionNamesCache.ContainsKey(key);
        }
        #endregion

        #region MetadataRegionNamesCache - MetadataRegionNamesCacheGet
        private List<string> MetadataRegionNamesCacheGet(MetadataRegionNameKey key)
        {
            lock (_metadataRegionNamesCacheLock) return metadataRegionNamesCache[key];
        }
        #endregion

        #region MetadataRegionNamesCache - MetadataRegionNamesCacheUpdate
        private void MetadataRegionNamesCacheUpdate(MetadataRegionNameKey key, List<string> regionNames)
        {
            //Update cache
            lock (_metadataRegionNamesCacheLock)
            {
                if (MetadataRegionNamesCacheContainsKey(key)) metadataRegionNamesCache[key] = regionNames;
                else metadataRegionNamesCache.Add(key, regionNames);
            }
        }
        #endregion

        #endregion

        #region Cache Metadata
        private static Dictionary<FileEntryBroker, Metadata> metadataCache = new Dictionary<FileEntryBroker, Metadata>();
        private static readonly Object metadataCacheLock = new Object();

        #region Cache Metadata - Read 
        public Metadata ReadMetadataFromCacheOrDatabase(FileEntryBroker fileEntryBroker)
        {
            //if (fileEntryBroker.GetType() != typeof(FileEntryBroker)) //Sometimes getting 'MetadataLibrary.FileEntryBroker' to type 'MetadataLibrary.FileEntryImage'
            fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result
            lock (metadataCacheLock) if (metadataCache.ContainsKey(fileEntryBroker)) return metadataCache[fileEntryBroker]; //Also return null
            
            Metadata metadata = Read(fileEntryBroker);
            MetadataCacheUpdate(fileEntryBroker, metadata);
            return metadata;
        }
        #endregion

        #region Cache Metadata - Read - CacheOnly
        public Metadata ReadMetadataFromCacheOnly(FileEntryBroker fileEntryBroker)
        {
            lock (metadataCacheLock)
            {
                if (fileEntryBroker.GetType() != typeof(FileEntryBroker)) fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result 
                if (metadataCache.ContainsKey(fileEntryBroker)) return metadataCache[fileEntryBroker]; //Also return null             
            }
            return null;
        }
        #endregion

        #region Cache Metadata - MetadataHasBeenRead
        public bool IsMetadataInCache(FileEntryBroker fileEntryBroker)
        {
            lock (metadataCacheLock)
            {
                if (fileEntryBroker.GetType() != typeof(FileEntryBroker)) fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result
                return metadataCache.ContainsKey(fileEntryBroker);
            }
        }
        #endregion 

        #region Cache Metadata - Updated    
        /// <summary>
        /// Updated the Metadata cache
        /// </summary>
        /// <param name="fileEntryBroker">When broler is MetadataBrokerType.ExifTool then don't remember null values, meands value not read</param>
        /// <param name="metadata">Matadata to remeber, also remember null values</param>
        private void MetadataCacheUpdate(FileEntryBroker fileEntryBroker, Metadata metadata)
        {
            if (fileEntryBroker.GetType() != typeof(FileEntryBroker)) fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result

            lock (metadataCacheLock)
            {
                if (metadataCache.ContainsKey(fileEntryBroker)) metadataCache[fileEntryBroker] = metadata;
                else metadataCache.Add(fileEntryBroker, metadata);
            }
            ListFileEntryAttributesCacheRemove(new FileBroker(fileEntryBroker.Broker, fileEntryBroker.FileFullPath));
            ListFileEntryAttributesCacheRemove(new FileBroker(fileEntryBroker.Broker | MetadataBrokerType.ExifToolWriteError, fileEntryBroker.FileFullPath));
        }
        #endregion

        #region Cache Metadata - Updated Region
        /// <summary>
        /// Find correct Metadata in cache, find and updated the Region in cache
        /// </summary>
        /// <param name="fileEntryBroker">Index of metadata to search for in cache</param>
        /// <param name="regionStructure">New region data</param>
        private void MetadataRegionCacheUpdate(Metadata metadata, RegionStructure regionStructure)
        {
            Metadata metadataCopy = new Metadata(metadata);
            MetadataCacheRemoveMetadataCacheRemove(metadata.FileEntryBroker);

            try
            {
                if (metadataCopy != null)
                {
                    lock (metadataCacheLock)
                    {
                        int indexRegionFound = -1;
                        for (int indexRegion = 0; indexRegion < metadataCopy.PersonalRegionList.Count; indexRegion++)
                        {
                            if (regionStructure == metadataCopy.PersonalRegionList[indexRegion])
                            {
                                indexRegionFound = indexRegion;
                                break;
                            }
                        }
                        if (indexRegionFound >= 0)
                        {
                            metadataCopy.PersonalRegionList.RemoveAt(indexRegionFound);
                            metadataCopy.PersonalRegionList.Insert(indexRegionFound, new RegionStructure(regionStructure));
                        }
                    }
                    MetadataCacheUpdate(metadata.FileEntryBroker, metadataCopy);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "MetadataRegionCacheUpdate");
            }
        }
        #endregion 

        #region Cache Metadata - Remove
        private void MetadataCacheRemoveMetadataCacheRemove(FileEntryBroker fileEntryBroker)
        {
            try
            {
                if (fileEntryBroker == null) return;
                if (fileEntryBroker.GetType() != typeof(FileEntryBroker)) fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result
                lock (metadataCacheLock)
                {
                    if (metadataCache.ContainsKey(fileEntryBroker)) metadataCache.Remove(fileEntryBroker);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "MetadataCacheRemoveMetadataCacheRemove");
            }
        }
        #endregion 

        #region Cache - ListFileEntryAttributesCacheRemove(FileBroker fileBroker)
        private void ListFileEntryAttributesCacheRemove(FileBroker fileBroker)
        {
            try
            {
                lock (_listFileAttributeDateVersionsLock)
                {
                    if (listFileAttributeDateVersions.ContainsKey(fileBroker)) listFileAttributeDateVersions.Remove(fileBroker);
                }
            } catch (Exception ex)
            {
                Logger.Error(ex, "ListFileEntryAttributesCacheRemove");
            }
        }
        #endregion

        #region Cache - Remove
        /// <summary>
        /// Remove metadata from Cache
        /// </summary>
        /// <param name="fileEntryBroker">Filename, Folder and Broker (also | MetadataBrokerType.ExifToolWriteError)</param>
        public void MetadataCacheRemove(FileEntryBroker fileEntryBroker)
        {
            List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();
            fileEntryBrokers.Add(fileEntryBroker);
            MetadataCacheRemove(fileEntryBrokers);
        }

        public void MetadataCacheRemove(List<FileEntryBroker> fileEntryBrokers)
        {
            MetadataRegionNamesCacheClear();
            ListAllPersonalRegionNameCountCacheClear();

            foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
            {
                MetadataCacheRemoveMetadataCacheRemove(fileEntryBroker);
                MetadataCacheRemoveMetadataCacheRemove(new FileEntryBroker(fileEntryBroker, MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError));
                ListFileEntryAttributesCacheRemove(new FileBroker(fileEntryBroker.Broker, fileEntryBroker.FileFullPath));
                ListFileEntryAttributesCacheRemove(new FileBroker(fileEntryBroker.Broker | MetadataBrokerType.ExifToolWriteError, fileEntryBroker.FileFullPath));
            }
        }
        #endregion

        #region Cache - Remove Folder + Filename (Copy and Move use this)
        public void MetadataCacheRemove(string directory, string fileName)
        {
            bool found;
            try
            {
                do
                {
                    found = false;
                    
                    FileEntryBroker fileEntryBrokerFound = null;
                    lock (metadataCacheLock)
                    {
                        foreach (FileEntryBroker fileEntryBroker in metadataCache.Keys)
                        {
                            if (fileEntryBroker.Directory == directory && fileEntryBroker.FileName == fileName)
                            {
                                fileEntryBrokerFound = fileEntryBroker;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (found) MetadataCacheRemove(fileEntryBrokerFound);

                } while (found);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "MetadataCacheRemove");
            }
        }
        #endregion 

        #region Cache - Remove Folder
        public void MetadataCacheRemove(string directory)
        {
            bool found;
            try
            {
                do
                {
                    found = false;
                    FileEntryBroker fileEntryBrokerFound = null;
                    lock (metadataCacheLock)
                    {
                        foreach (FileEntryBroker fileEntryBroker in metadataCache.Keys)
                        {
                            if (fileEntryBroker.Directory == directory)
                            {
                                fileEntryBrokerFound = fileEntryBroker;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (found) MetadataCacheRemove(fileEntryBrokerFound);

                } while (found);
            } catch (Exception ex)
            {
                Logger.Error(ex, "MetadataCacheRemove");
            }
        }
        #endregion 

        #region Cache - Remove Folder + MetadataBrokerType
        public void MetadataCacheRemove(MetadataBrokerType broker, string directory, DateTime dateTime)
        {
            List<FileEntryBroker> foundKeys = new List<FileEntryBroker>();
            try
            {
                lock (metadataCacheLock)
                {
                    foreach (FileEntryBroker fileEntryBroker in metadataCache.Keys)
                    {
                        if (fileEntryBroker.Broker == broker && fileEntryBroker.Directory == directory && fileEntryBroker.LastWriteDateTime == dateTime)
                        {
                            foundKeys.Add(fileEntryBroker);
                        }
                    }
                }
                foreach (FileEntryBroker fileEntryBrokerRemove in foundKeys) MetadataCacheRemove(fileEntryBrokerRemove);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "MetadataCacheRemove");
            }
        }
        #endregion 

        #endregion
    }
}
