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
using System.Drawing.Drawing2D;

namespace Thumbnails
{
    public class ThumbnailDatabaseCache
    {
        public static bool StopCaching { 
            get; 
            set; 
        } = false;
        private SqliteDatabaseUtilities dbTools;
        public ThumbnailDatabaseCache(SqliteDatabaseUtilities databaseTools)
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

        #region Thumbnail
        public Size UpsizeThumbnailSize { get; set; } = new Size(192, 192);

        #region Thumbnail - WriteThumbnail
        public void WriteThumbnail(FileEntry fileEntry, Image image) 
        {
            //Don't do DeleteThumbnail(fileDirectory, fileName, size); //It create a lot overhead
            //Do to read thumbnail only write back what doesn't exist
            string sqlCommand =
                "INSERT INTO MediaThumbnail (FileDirectory, FileName, FileDateModified,Image) " +
                "Values (@FileDirectory, @FileName, @FileDateModified, @Image)";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));
                commandDatabase.Parameters.AddWithValue("@Image", dbTools.ImageToByteArray(image));
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            ThumbnailCacheUpdate(fileEntry, image);
        }
        #endregion

        #region Thumbnail - Read 
        public Image Read(FileEntry fileEntry)
        {
            HashSet<FileEntry> fileEntriesPutInCache = new HashSet<FileEntry>();
            fileEntriesPutInCache.Add(fileEntry);
            ReadToCache(fileEntriesPutInCache);
            return ReadThumbnailFromCacheOnlyClone(fileEntry);

        }
        #endregion

        #region ReadToCache(List<FileEntry> fileEntries)
        public void ReadToCache(HashSet<FileEntry> fileEntries)
        {        
            List<FileEntry> fileEntriesPutInCache = new List<FileEntry>();
            foreach (FileEntry fileEntryToCheckInCache in fileEntries)
            {
                Image image = ReadThumbnailFromCacheOnlyClone(fileEntryToCheckInCache);
                if (image == null) fileEntriesPutInCache.Add(fileEntryToCheckInCache);
            }

            string sqlCommand =
                "SELECT Image FROM MediaThumbnail WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
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

        }
        #endregion 

        #region Thumbnail - Read 
        private static List<string> readFolderToCacheCached = new List<string>();
        public void ReadToCacheFolder(string directory)
        {
            if (readFolderToCacheCached.Contains(directory)) return;
            readFolderToCacheCached.Add(directory);

            string sqlCommand = "SELECT FileDirectory, FileName, FileDateModified, Image FROM MediaThumbnail";
            if (!string.IsNullOrWhiteSpace(directory)) sqlCommand += " WHERE FileDirectory = @FileDirectory";
            
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
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
        }
        #endregion

        #region Thumbnail - DeleteThumbnail
        public void DeleteThumbnails(List<FileEntry> fileEntries)
        {
            string sqlCommand = "DELETE FROM MediaThumbnail WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
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
            
        }
        #endregion 

        #region Thumbnail - DeleteDirectory
        public int DeleteDirectoryAndHistory(string fileDirectory)
        {
            int rowsAffected = 0;
            ThumbnailClearCache();
            string sqlCommand = "DELETE FROM MediaThumbnail WHERE FileDirectory = @FileDirectory";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                rowsAffected = commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            return rowsAffected;
        }
        #endregion 

        #region Thumbnail - ListFileEntryDateVersions
        public List<FileEntry> ListFileEntryDateVersions(string fullFileName)
        {
            return ListFileEntryDateVersions(Path.GetDirectoryName(fullFileName), Path.GetFileName(fullFileName));
        }
        #endregion

        #region Thumbnail - ListFileEntryDateVersions
        public List<FileEntry> ListFileEntryDateVersions(string fileDirectory, string fileName)
        {
            List<FileEntry> fileEntries = new List<FileEntry>();

            string sqlCommand = "SELECT DISTINCT FileDirectory, FileName, FileDateModified FROM MediaThumbnail " +
                "WHERE FileDirectory = @FileDirectory AND FileName = @FileName";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
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
            return fileEntries;
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

        

        #endregion 

        #region Thumbnail - Cache
        private static Dictionary<FileEntry, Image> thumbnailCache = new Dictionary<FileEntry, Image>();
        private static readonly Object thumbnailCacheLock = new Object();

        #region Thumbnail - DoesThumbnailExist
        public bool DoesThumbnailExist(FileEntry fileEntry)
        {
            lock (thumbnailCacheLock) if (thumbnailCache.ContainsKey(fileEntry)) return true;
            return ReadThumbnailFromCacheOrDatabase(fileEntry) != null; //Read Thumbnail and put in cache, will need the thumbnail soon anywhy 
        }
        #endregion

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

        #region Thumbnail - Cache - ReadThumbnailFromCacheOnlyClone
        public Image ReadThumbnailFromCacheOnlyClone(FileEntry fileEntry)
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
