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
using System.Collections.Generic;
using System.IO;
using MetadataLibrary;
using SqliteDatabase;
using NLog;

namespace Exiftool
{
    public class ExiftoolDataDatabase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private SqliteDatabaseUtilities dbTools;
        public ExiftoolDataDatabase(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }

        #region Table: MediaExiftoolTags

        public List<ExiftoolData> Read(FileEntry file)
        {
            List<ExiftoolData> exifToolDataList = new List<ExiftoolData>();

            var sqlTransactionSelect = dbTools.TransactionBeginSelect();

            #region SELECT FROM MediaExiftoolTags
            string sqlCommand = "SELECT FileDirectory, FileName, FileDateModified, Region, Command, Parameter FROM " +
                "MediaExiftoolTags WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(file.FileFullPath));
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(file.FileFullPath));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(file.LastWriteDateTime));
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ExiftoolData exifToolData = new ExiftoolData();
                        exifToolData.FileDirectory = dbTools.ConvertFromDBValString(reader["FileDirectory"]);
                        exifToolData.FileName = dbTools.ConvertFromDBValString(reader["FileName"]);
                        exifToolData.FileDateModified = (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]);
                        exifToolData.Region = dbTools.ConvertFromDBValString(reader["Region"]);
                        exifToolData.Command = dbTools.ConvertFromDBValString(reader["Command"]);
                        exifToolData.Parameter = dbTools.ConvertFromDBValString(reader["Parameter"]);
                        exifToolDataList.Add(exifToolData);
                    }
                }
            }
            #endregion

            dbTools.TransactionCommitSelect(sqlTransactionSelect);

            return exifToolDataList;
        }

        public bool Write(ExiftoolData exifToolData)
        {
            bool success = false;
            var sqlTransaction = dbTools.TransactionBeginBatch();

            #region INSERT INTO MediaExiftoolTags
            string sqlCommand =
                "INSERT INTO MediaExiftoolTags (FileDirectory, FileName, FileDateModified, Region, Command, Parameter) " +
                "Values (@FileDirectory, @FileName, @FileDateModified, @Region, @Command, @Parameter)";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", exifToolData.FileDirectory);
                commandDatabase.Parameters.AddWithValue("@FileName", exifToolData.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(exifToolData.FileDateModified));
                commandDatabase.Parameters.AddWithValue("@Region", exifToolData.Region);
                commandDatabase.Parameters.AddWithValue("@Command", exifToolData.Command);
                commandDatabase.Parameters.AddWithValue("@Parameter", exifToolData.Parameter);

                if (commandDatabase.ExecuteNonQuery() != -1) success = true;
                if (success)
                {
                    Logger.Error("Delete MediaExiftoolTags data due to previous application crash for file: " + exifToolData.FullFilePath);
                    //Delete all extries due to crash.
                    DeleteFileEntryMediaExiftoolTags(new FileEntry(exifToolData.FileDirectory, exifToolData.FileName, exifToolData.FileDateModified));
                    commandDatabase.ExecuteNonQuery();
                }   // Execute the query
            }
            #endregion

            dbTools.TransactionCommitBatch(sqlTransaction);
            return success;
        }

        public int DeleteDirectoryAndHistory(string fileDirectory)
        {
            int recordAffected = 0;

            var sqlTransaction = dbTools.TransactionBeginBatch();

            #region DELETE FROM MediaExiftoolTags 
            string sqlCommand = "DELETE FROM MediaExiftoolTags WHERE FileDirectory = @FileDirectory";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                recordAffected = commandDatabase.ExecuteNonQuery();      // Execute the query
            }
            #endregion

            dbTools.TransactionCommitBatch(sqlTransaction);
            
            return recordAffected;
        }

        private void DeleteFileEntryMediaExiftoolTags(FileEntry fileEntry)
        {
            List<FileEntry> fileEntries = new List<FileEntry>();
            fileEntries.Add(fileEntry);
            DeleteFileEntriesFromMediaExiftoolTags(fileEntries);
        }

        public void DeleteFileEntriesFromMediaExiftoolTags(List<FileEntry> fileEntries)
        {
            var sqlTransaction = dbTools.TransactionBeginBatch();

            #region DELETE FROM MediaExiftoolTags 
            string sqlCommand = "DELETE FROM MediaExiftoolTags " +
                "WHERE FileDirectory = @FileDirectory " +
                "AND FileName = @FileName " +
                "AND FileDateModified = @FileDateModified ";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                foreach (FileEntry fileEntry in fileEntries)
                {
                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                    commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));
                    commandDatabase.ExecuteNonQuery();      // Execute the query
                }
            }
            #endregion

            dbTools.TransactionCommitBatch(sqlTransaction);
        }


        public List<FileEntryAttribute> ListFileEntryDateVersions(string fileName)
        {
            return ListFileEntryDateVersions(Path.GetFileName(fileName), Path.GetDirectoryName(fileName));
        }

        public List<FileEntryAttribute> ListFileEntryDateVersions(string fileName, string fileDirectory)
        {
            List<FileEntryAttribute> exifToolDates = new List<FileEntryAttribute>();

            var sqlTransactionSelect = dbTools.TransactionBeginSelect();

            #region SELECT DISTINCT FileDirectory, FileName, FileDateModified FROM MediaExiftoolTags 
            string sqlCommand = "SELECT DISTINCT FileDirectory, FileName, FileDateModified FROM MediaExiftoolTags " + 
                "WHERE FileDirectory = @FileDirectory AND FileName = @FileName";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                FileEntryAttribute newstFileEntryAttributeForEdit = null;

                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileName);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool isErrorVersion = false;
                        DateTime currentMetadataDate = (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]);

                        FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            currentMetadataDate, FileEntryVersion.Historical);
                        exifToolDates.Add(fileEntryAttribute);

                        if (!isErrorVersion && (newstFileEntryAttributeForEdit == null || currentMetadataDate > newstFileEntryAttributeForEdit.LastWriteDateTime))
                        {
                            newstFileEntryAttributeForEdit = new FileEntryAttribute((FileEntry)fileEntryAttribute, FileEntryVersion.CurrentVersionInDatabase);
                        }
                    }

                    if (newstFileEntryAttributeForEdit != null) exifToolDates.Add(newstFileEntryAttributeForEdit);
                }

                exifToolDates.Sort();
            }
            #endregion

            dbTools.TransactionCommitSelect(sqlTransactionSelect);

            return exifToolDates;
        }


        #endregion 

    }
}
