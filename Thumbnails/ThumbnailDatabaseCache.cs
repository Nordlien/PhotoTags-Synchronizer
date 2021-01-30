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
            dbTools.TransactionCommitBatch();
        }
        public Size UpsizeThumbnailSize { get; set; } = new Size(192, 192);


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
            CacheUpdate(fileEntry, image);
        }

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

            return image; //== null ? null : Manina.Windows.Forms.Utility.ThumbnailFromImage(image, UpsizeThumbnailSize, Color.White, true);
        }

        

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
            CacheRemove(fileEntry);
        }

        public void DeleteDirectory(string fileDirectory)
        {
            ClearCache();
            string sqlCommand = "DELETE FROM MediaThumbnail WHERE FileDirectory = @FileDirectory";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        public List<FileEntry> ListFileEntryDateVersions(string fullFileName)
        {
            return ListFileEntryDateVersions(Path.GetDirectoryName(fullFileName), Path.GetFileName(fullFileName));
        }

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
        public bool DoesThumbnailExist(FileEntry fileEntry)
        {
            if (CacheContainsKey(fileEntry)) return true;
            return ReadThumbnailFromCacheOrDatabase(fileEntry) != null; //Read Thumbnail and put in cache, will need the thumbnail soon anywhy 
        }


        #region Cache
        Dictionary<FileEntry, Image> thumbnailCache = new Dictionary<FileEntry, Image>();

        private void CacheUpdate(FileEntry fileEntry, Image image)
        {
            if (!(fileEntry is FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            if (thumbnailCache.ContainsKey(fileEntry))
            {
                thumbnailCache[fileEntry] = image;
            }
            else
            {
                thumbnailCache.Add(fileEntry, image);
            }
        }

        private bool CacheContainsKey(FileEntry fileEntry)
        {
            if (!(fileEntry is FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            return thumbnailCache.ContainsKey(fileEntry);
        }

        private Image CacheGet(FileEntry fileEntry)
        {
            if (!(fileEntry is FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            return thumbnailCache[fileEntry];
        }

        public void CacheRemove(FileEntry fileEntry)
        {
            if (fileEntry == null) return;
            if (!(fileEntry is FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result

            if (CacheContainsKey(fileEntry))
            {
                thumbnailCache.Remove(fileEntry);
            }
        }

        public void CacheRemove(FileEntry[] fileEntries)
        {
            if (fileEntries == null) return;
            foreach (FileEntry fileEntry in fileEntries)
            {
                if (CacheContainsKey(fileEntry))
                {
                    thumbnailCache.Remove(fileEntry);
                }
            }
        }

        public void ClearCache()
        {
            thumbnailCache = null;
            thumbnailCache = new Dictionary<FileEntry, Image>();
        }

        public bool DoesThumbnailExistInCache(FileEntry fileEntry)
        {
            if (!(fileEntry is FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            return CacheContainsKey(fileEntry);
        }
        public Image ReadThumbnailFromCacheOnly(FileEntry fileEntry)
        {
            if (!(fileEntry is FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            if (CacheContainsKey(fileEntry))
            {
                return CacheGet(fileEntry);
            }
            return null;
        }

        public Image ReadThumbnailFromCacheOnlyClone(FileEntry fileEntry)
        {
            if (!(fileEntry is FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            if (CacheContainsKey(fileEntry))
            {
                return CacheGet(fileEntry);
                //return new Bitmap(CacheGet(file));
            }
            return null;
        }

        public Image ReadThumbnailFromCacheOrDatabase(FileEntry fileEntry)
        {
            if (!(fileEntry is FileEntry)) fileEntry = new FileEntry(fileEntry); //When NOT FileEntry it Will give wrong hash value, and wrong key and wrong result
            if (CacheContainsKey(fileEntry)) return CacheGet(fileEntry);
            
            Image image;
            image = Read(fileEntry);
            if (image != null)
            {
                CacheUpdate(fileEntry, image);
                return image;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
