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
            return image;
        }

        public bool DoesThumbnailExist(FileEntry fileEntry)
        {
            object exisit = null;

            string sqlCommand =
                "SELECT 1 FROM MediaThumbnail WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));
                commandDatabase.Prepare();

                exisit = commandDatabase.ExecuteScalar();
            }
            return exisit != null;
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

        #region Cache
        Dictionary<FileEntry, Image> thumbnailCache = new Dictionary<FileEntry, Image>();

        private void CacheUpdate(FileEntry file, Image image)
        {
            
            if (thumbnailCache.ContainsKey(file))
            {
                thumbnailCache[file] = image;
            }
            else
            {
                thumbnailCache.Add(file, image);
            }
        }

        private bool CacheContainsKey(FileEntry file)
        {
            return thumbnailCache.ContainsKey(file);
        }

        private Image CacheGet(FileEntry file)
        {
            return thumbnailCache[file];
        }

        public void CacheRemove(FileEntry file)
        {
            if (file == null) return;
            if (CacheContainsKey(file))
            {
                thumbnailCache.Remove(file);
            }
        }

        public void CacheRemove(FileEntry[] files)
        {
            if (files == null) return;
            foreach (FileEntryBroker file in files)
            {
                if (CacheContainsKey(file))
                {
                    thumbnailCache.Remove(file);
                }
            }
        }

        public void ClearCache()
        {
            thumbnailCache = null;
            thumbnailCache = new Dictionary<FileEntry, Image>();
        }


        public Image ReadCache(FileEntry file)
        {
            if (CacheContainsKey(file))
            {
                return CacheGet(file);
            }

            Image image;

            image = Read(file);
            if (image != null)
            {
                CacheUpdate(file, image);
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
