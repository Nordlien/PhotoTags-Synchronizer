#define MicrosoftDataSqlite

#if MonoSqlite
using Mono.Data.Sqlite;
#elif MicrosoftDataSqlite
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif
using SqliteDatabase;
using System.Drawing;
using System.IO;
using MetadataLibrary;
using System.Collections.Generic;
using System;

namespace Thumbnails
{
    public class ThumbnailPosterDatabaseCache
    {
        public static bool StopCaching { 
            get; 
            set; 
        } = false;

        private SqliteDatabaseUtilities dbTools;
        public ThumbnailPosterDatabaseCache(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools; 
        }

        public Size UpsizeThumbnailSize { get; set; } = new Size(192, 192);

        #region Thumbnail - WriteThumbnail
        public void WriteThumbnail(FileEntry fileEntry, Image image) 
        {
            //Don't do DeleteThumbnail(fileDirectory, fileName, size); //It create a lot overhead
            //Do to read thumbnail only write back what doesn't exist

            var sqlTransaction = dbTools.TransactionBeginBatch();

            #region INSERT INTO MediaThumbnail
            string sqlCommand =
                "INSERT INTO MediaThumbnail (FileDirectory, FileName, FileDateModified,Image) " +
                "Values (@FileDirectory, @FileName, @FileDateModified, @Image)";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));
                commandDatabase.Parameters.AddWithValue("@Image", dbTools.ImageToByteArray(image));
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            #endregion

            dbTools.TransactionCommitBatch(sqlTransaction);
            
            ThumbnailCacheUpdate(fileEntry, image);
        }
        #endregion

        #region Thumbnail - Read 
        public Image Read(FileEntry fileEntry)
        {
            HashSet<FileEntry> fileEntriesPutInCache = new HashSet<FileEntry>();
            fileEntriesPutInCache.Add(fileEntry);
            ReadToCache(fileEntriesPutInCache);
            return ReadThumbnailFromCacheOnly(fileEntry);

        }
        #endregion

        #region Move Metadata
        public bool Move(string oldDirectory, string oldFilename, string newDirectory, string newFilename)
        {
            bool movedOk = true;
            ThumnbailCacheRemove(oldDirectory, oldFilename);

            string oldPath = Path.Combine(oldDirectory, oldFilename).ToLower();
            string newPath = Path.Combine(newDirectory, newFilename).ToLower();
            if (string.Compare(oldPath, newPath, true) != 0)
            {
                var sqlTransaction = dbTools.TransactionBeginBatch();
                //CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction)
                //CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
                #region UPDATE MediaThumbnail 
                string sqlCommand =
                           "UPDATE MediaThumbnail SET " +
                           "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                           "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
                using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
                {
                    //commandDatabase.Prepare();
                    //commandDatabase.Parameters.AddWithValue("@Broker", (int)broker);
                    commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                    commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                    commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                    commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                    if (commandDatabase.ExecuteNonQuery() == -1) movedOk = false;
                }
                #endregion

                dbTools.TransactionCommitBatch(sqlTransaction);
            }
            
            
            return movedOk;
        }
        #endregion

        #region ReadToCache(List<FileEntry> fileEntries)
        public void ReadToCache(HashSet<FileEntry> fileEntries)
        {        
            List<FileEntry> fileEntriesPutInCache = new List<FileEntry>();
            foreach (FileEntry fileEntryToCheckInCache in fileEntries)
            {
                Image image = ReadThumbnailFromCacheOnly(fileEntryToCheckInCache);
                if (image == null) fileEntriesPutInCache.Add(fileEntryToCheckInCache);
            }

            var sqlTransactionSelect = dbTools.TransactionBeginSelect();
            
            #region SELECT Image FROM MediaThumbnail 
            string sqlCommand =
                "SELECT Image FROM MediaThumbnail WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
            {
                foreach (FileEntry fileEntry in fileEntriesPutInCache)
                {
                    if (StopCaching) { StopCaching = false; return; }
                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                    commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));

                    using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Image image = dbTools.ByteArrayToImage(dbTools.ConvertFromDBValByteArray(reader["Image"]));
                            ThumbnailCacheUpdate(fileEntry, image);
                        }
                    }
                }
            }
            #endregion

            dbTools.TransactionCommitSelect(sqlTransactionSelect);
        }
        #endregion 

        #region Thumbnail - Read 
        private static List<string> readFolderToCacheCached = new List<string>();
        public void ReadToCacheFolder(string directory)
        {
            if (readFolderToCacheCached.Contains(directory)) return;
            readFolderToCacheCached.Add(directory);

            var sqlTransactionSelect = dbTools.TransactionBeginSelect();

            #region SELECT FileDirectory, FileName, FileDateModified, Image FROM MediaThumbnail
            string sqlCommand = "SELECT FileDirectory, FileName, FileDateModified, Image FROM MediaThumbnail";
            if (!string.IsNullOrWhiteSpace(directory)) sqlCommand += " WHERE FileDirectory = @FileDirectory";
            
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
            {
                if (!string.IsNullOrWhiteSpace(directory)) commandDatabase.Parameters.AddWithValue("@FileDirectory", directory);
                
                //commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (StopCaching) {
                            try
                            {
                                readFolderToCacheCached.Remove(directory);
                            }
                            catch { }
                            StopCaching = false; 
                            return; 
                        }
                        FileEntry fileEntry = new FileEntry(
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"])
                            );
                        Image image = dbTools.ByteArrayToImage(dbTools.ConvertFromDBValByteArray(reader["Image"]));
                        ThumbnailCacheUpdate(fileEntry, image);
                    }
                }
            }
            #endregion

            dbTools.TransactionCommitSelect(sqlTransactionSelect);
        }
        #endregion

        #region Thumbnail - DeleteThumbnail
        public void DeleteThumbnails(List<FileEntry> fileEntries)
        {
            var sqlTransaction = dbTools.TransactionBeginBatch();

            #region DELETE FROM MediaThumbnail 
            string sqlCommand = "DELETE FROM MediaThumbnail WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
            {
                foreach (FileEntry fileEntry in fileEntries)
                {
                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                    commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));
                    commandDatabase.ExecuteNonQuery();      // Execute the query
                    ThumnbailCacheRemove(fileEntry);
                }
            }
            #endregion

            dbTools.TransactionCommitBatch(sqlTransaction);
        }
        #endregion 

        #region Thumbnail - DeleteDirectory
        public int DeleteDirectoryAndHistory(string fileDirectory)
        {
            int rowsAffected = 0;
            ThumbnailClearCache();
            
            var sqlTransaction = dbTools.TransactionBeginBatch();
            
            #region DELETE FROM MediaThumbnail 
            string sqlCommand = "DELETE FROM MediaThumbnail WHERE FileDirectory = @FileDirectory";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                rowsAffected = commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            #endregion

            dbTools.TransactionCommitBatch(sqlTransaction);
            
            return rowsAffected;
        }
        #endregion

        #region Thumbnail - SELECT - ListFileEntryDateVersions
        public List<FileEntry> ListFileEntryDateVersions(string fileDirectory, string fileName)
        {
            List<FileEntry> fileEntries = new List<FileEntry>();

            var sqlTransactionSelect = dbTools.TransactionBeginSelect();

            #region SELECT DISTINCT FileDirectory, FileName, FileDateModified FROM MediaThumbnail
            string sqlCommand = "SELECT DISTINCT FileDirectory, FileName, FileDateModified FROM MediaThumbnail " +
                "WHERE FileDirectory = @FileDirectory AND FileName = @FileName";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileName);

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FileEntry fileEntry = new FileEntry
                            (
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"])
                            );
                        fileEntries.Add(fileEntry);
                    }
                }

                //fileEntries.Sort();
            }
            #endregion

            dbTools.TransactionCommitSelect(sqlTransactionSelect);

            return fileEntries;
        }
        #endregion 
        #region Thumbnail - ListFileEntryDateVersions
        public List<FileEntry> ListFileEntryDateVersions(string fullFileName)
        {
            return ListFileEntryDateVersions(Path.GetDirectoryName(fullFileName), Path.GetFileName(fullFileName));
        }
        #endregion

        #region Thumbnail - DoesMetadataMissThumbnailInRegion
        public bool DoesMetadataMissThumbnailInRegion(Metadata metadata)
        {
            bool needCreateThumbnail = false;
            if (metadata != null)
            {
                foreach (RegionStructure regionStructure in metadata.PersonalRegionList)
                {
                    if (regionStructure.Thumbnail == null)
                    {
                        needCreateThumbnail = true;
                        break;
                    }
                }
            }
            return needCreateThumbnail;
        }
        #endregion

        #region Thumbnail - Cache
        private static Dictionary<FileEntry, Image> thumbnailCache = new Dictionary<FileEntry, Image>();
        private static readonly Object thumbnailCacheLock = new Object();

        #region Thumbnail - Cache - ThumbnailCacheUpdate
        public void ThumbnailCacheUpdate(FileEntry fileEntry, Image image)
        {
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            lock (thumbnailCacheLock) if(thumbnailCache.ContainsKey(fileEntry)) thumbnailCache[fileEntry] = image;
            else thumbnailCache.Add(fileEntry, image);            
        }
        #endregion  

        #region Thumbnail - Cache - ThumnbailCacheRemove
        public void ThumnbailCacheRemove(FileEntry fileEntry)
        {
            if (fileEntry == null) return;
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            lock (thumbnailCacheLock) if (thumbnailCache.ContainsKey(fileEntry)) thumbnailCache.Remove(fileEntry);
        }
        #endregion 

        #region Thumbnail - Cache - Remove Folder + Filename (Copy and Move use this)
        public void ThumnbailCacheRemove(string directory, string fileName)
        {
            bool found;
            try
            {
                do
                {
                    found = false;

                    FileEntry fileEntryFound = null;
                    lock (thumbnailCacheLock)
                    {
                        foreach (FileEntry fileEntry in thumbnailCache.Keys)
                        {
                            if (String.Compare(fileEntry.Directory, directory, comparisonType: StringComparison.OrdinalIgnoreCase) == 0 &&
                                String.Compare(fileEntry.FileName, fileName, comparisonType: StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                fileEntryFound = fileEntry;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (found) ThumnbailCacheRemove(fileEntryFound);

                } while (found);
            }
            catch //(Exception ex)
            {
                //Logger.Error(ex, "ThumnbailCacheRemove");
            }
        }
        #endregion 

        #region Thumbnail - Cache - ThumbnailClearCache
        public void ThumbnailClearCache()
        {
            thumbnailCache = null;
            thumbnailCache = new Dictionary<FileEntry, Image>();
            readFolderToCacheCached.Clear(); //Don't clear the folder cache when only one or few thumbnails has been updated, that will force system to reread folder all the time
        }
        #endregion 

        #region Thumbnail - Cache - DoesThumbnailExistInCache
        public bool DoesThumbnailExistInCache(FileEntry fileEntry)
        {
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            lock (thumbnailCacheLock) return thumbnailCache.ContainsKey(fileEntry);
        }
        #endregion 

        #region Thumbnail - Cache - ReadThumbnailFromCacheOnly
        public Image ReadThumbnailFromCacheOnly(FileEntry fileEntry)
        {
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            lock (thumbnailCacheLock) if (thumbnailCache.ContainsKey(fileEntry)) return thumbnailCache[fileEntry]; //Testing without clone, looks as unsafe code gone            
            return null;
        }
        #endregion 

        #region Thumbnail - Cache - ReadThumbnailFromCacheOrDatabase
        public Image ReadThumbnailFromCacheOrDatabase(FileEntry fileEntry)
        {
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result            
            lock (thumbnailCacheLock) if (thumbnailCache.ContainsKey(fileEntry)) return new Bitmap(thumbnailCache[fileEntry]);
            
            Image image = Read(fileEntry);
            if (image != null) ThumbnailCacheUpdate(fileEntry, new Bitmap(image));
            return image;
        }
        #endregion

        #endregion

        
    }
}
