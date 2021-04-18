#define MonoSqlite
#if MonoSqlite
using Mono.Data.Sqlite;
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
    public class ThumbnailDatabaseCache
    {

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
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));
                commandDatabase.Parameters.AddWithValue("@Image", dbTools.ImageToByteArray(image));
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            ThumbnailCacheUpdate(fileEntry, image);
        }
        #endregion

        #region Thumbnail - Read 
        public Image Read(FileEntry fileEntry)
        {
            Image image = null;

            string sqlCommand =
                "SELECT Image FROM MediaThumbnail WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        image = dbTools.ByteArrayToImage(dbTools.ConvertFromDBValByteArray(reader["Image"]));
                    }
                }
            }
            return image; 
        }
        #endregion

        #region Thumbnail - Read 
        public Image ReadDirectory(string directory)
        {
            Image image = null;

            string sqlCommand = "SELECT FileDirectory, FileName, FileDateModified, Image FROM MediaThumbnail";
            if (!string.IsNullOrWhiteSpace(directory)) sqlCommand += " WHERE FileDirectory = @FileDirectory";
            
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                if (!string.IsNullOrWhiteSpace(directory)) commandDatabase.Parameters.AddWithValue("@FileDirectory", directory);
                
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FileEntry fileEntry = new FileEntry(
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"])
                            );
                        image = dbTools.ByteArrayToImage(dbTools.ConvertFromDBValByteArray(reader["Image"]));
                        ThumbnailCacheUpdate(fileEntry, image);
                    }
                }
            }
            return image;
        }
        #endregion

        #region Thumbnail - DeleteThumbnail
        public void DeleteThumbnail(FileEntry fileEntry)
        {
            string sqlCommand = "DELETE FROM MediaThumbnail WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            ThumnbailCacheRemove(fileEntry);
        }
        #endregion 

        #region Thumbnail - DeleteDirectory
        public void DeleteDirectory(string fileDirectory)
        {
            ThumbnailClearCache();
            string sqlCommand = "DELETE FROM MediaThumbnail WHERE FileDirectory = @FileDirectory";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
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
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileName);
                commandDatabase.Prepare();

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

                fileEntries.Sort();
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
        Dictionary<FileEntry, Image> thumbnailCache = new Dictionary<FileEntry, Image>();

        

        #region Thumbnail - DoesThumbnailExist
        public bool DoesThumbnailExist(FileEntry fileEntry)
        {
            if (thumbnailCache.ContainsKey(fileEntry)) return true;
            return ReadThumbnailFromCacheOrDatabase(fileEntry) != null; //Read Thumbnail and put in cache, will need the thumbnail soon anywhy 
        }
        #endregion

        #region Thumbnail - Cache - ThumbnailCacheUpdate
        private void ThumbnailCacheUpdate(FileEntry fileEntry, Image image)
        {
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            if (thumbnailCache.ContainsKey(fileEntry)) thumbnailCache[fileEntry] = image;
            else thumbnailCache.Add(fileEntry, image);
        }
        #endregion  

        #region Thumbnail - Cache - ThumnbailCacheRemove
        public void ThumnbailCacheRemove(FileEntry fileEntry)
        {
            if (fileEntry == null) return;
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            if (thumbnailCache.ContainsKey(fileEntry)) thumbnailCache.Remove(fileEntry);
        }
        #endregion 

        #region Thumbnail - Cache - ThumbnailClearCache
        public void ThumbnailClearCache()
        {
            thumbnailCache = null;
            thumbnailCache = new Dictionary<FileEntry, Image>();
        }
        #endregion 

        #region Thumbnail - Cache - DoesThumbnailExistInCache
        public bool DoesThumbnailExistInCache(FileEntry fileEntry)
        {
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            return thumbnailCache.ContainsKey(fileEntry);
        }
        #endregion 

        #region Thumbnail - Cache - ReadThumbnailFromCacheOnlyClone
        public Image ReadThumbnailFromCacheOnlyClone(FileEntry fileEntry)
        {
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            if (thumbnailCache.ContainsKey(fileEntry)) return thumbnailCache[fileEntry]; //Testing without clone, looks as unsafe code gone            
            return null;
        }
        #endregion 

        #region Thumbnail - Cache - ReadThumbnailFromCacheOrDatabase
        public Image ReadThumbnailFromCacheOrDatabase(FileEntry fileEntry)
        {
            if (fileEntry.GetType() != typeof(FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result            
            if (thumbnailCache.ContainsKey(fileEntry)) return thumbnailCache[fileEntry];
            
            Image image = Read(fileEntry);
            if (image != null) ThumbnailCacheUpdate(fileEntry, image);
            return image;
        }
        #endregion

        #endregion


        #region Region - UpdateRegionThumbnail
        public void UpdateRegionThumbnail(FileEntryBroker file, RegionStructure region)
        {
            //ThumnbailCacheRemove(file);

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
                commandDatabase.Parameters.AddWithValue("@Broker", (int)file.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", file.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", file.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(file.LastWriteDateTime));
                commandDatabase.Parameters.AddWithValue("@Type", region.Type);
                commandDatabase.Parameters.AddWithValue("@Name", region.Name);
                commandDatabase.Parameters.AddWithValue("@AreaX", region.AreaX);
                commandDatabase.Parameters.AddWithValue("@AreaY", region.AreaY);
                commandDatabase.Parameters.AddWithValue("@AreaWidth", region.AreaWidth);
                commandDatabase.Parameters.AddWithValue("@AreaHeight", region.AreaHeight);
                commandDatabase.Parameters.AddWithValue("@RegionStructureType", (int)region.RegionStructureType);

                if (region.Thumbnail == null)
                    commandDatabase.Parameters.AddWithValue("@Thumbnail", DBNull.Value);
                else commandDatabase.Parameters.AddWithValue("@Thumbnail", dbTools.ImageToByteArray(region.Thumbnail));

                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }
        }
        #endregion

    }
}
