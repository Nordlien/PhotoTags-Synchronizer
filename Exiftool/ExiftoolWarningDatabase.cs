﻿#define MonoSqlite
#if MonoSqlite
#else
using System.Data.SQLite;
#endif

using MetadataLibrary;
using SqliteDatabase;
using System;
using System.Collections.Generic;
using System.IO;

namespace Exiftool
{
    public class ExiftoolWarningDatabase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private SqliteDatabaseUtilities dbTools;
        public ExiftoolWarningDatabase(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }

        #region Read
        public List<ExiftoolWarningData> Read(FileEntry fileEntry)
        {
            List<ExiftoolWarningData> exifToolDataList = new List<ExiftoolWarningData>();

            Mono.Data.Sqlite.SqliteTransaction sqlTransactionSelect;
            do
            {
                sqlTransactionSelect = dbTools.TransactionBeginSelect();

                #region SELECT MediaExiftoolTagsWarning
                string sqlCommand = "SELECT FileDirectory, FileName, FileDateModified, OldRegion, OldCommand, OldParameter, NewRegion, NewCommand, NewParameter, Warning FROM " +
                "MediaExiftoolTagsWarning WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified=@FileDateModified";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fileEntry.FileFullPath));
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fileEntry.FileFullPath));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime)); 
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ExiftoolWarningData exifToolWarningData = new ExiftoolWarningData();
                        exifToolWarningData.OldExiftoolData = new ExiftoolData(
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]), 
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]),
                            dbTools.ConvertFromDBValString(reader["OldRegion"]),
                            dbTools.ConvertFromDBValString(reader["OldCommand"]),
                            dbTools.ConvertFromDBValString(reader["OldParameter"]), null);
                        exifToolWarningData.NewExiftoolData = new ExiftoolData(
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]),
                            dbTools.ConvertFromDBValString(reader["NewRegion"]),
                            dbTools.ConvertFromDBValString(reader["NewCommand"]),
                            dbTools.ConvertFromDBValString(reader["NewParameter"]), null);
                        exifToolWarningData.WarningMessage = dbTools.ConvertFromDBValString(reader["Warning"]);
                        exifToolDataList.Add(exifToolWarningData);
                    }
                }
            }
                #endregion

            } while (!dbTools.TransactionCommitSelect(sqlTransactionSelect));

            return exifToolDataList;
        }
        #endregion

        #region Write
        public void Write(ExiftoolData exifToolOldValue, ExiftoolData exifToolNewValue, string warning)
        {
            int resultRowsAffected = 1;
            Mono.Data.Sqlite.SqliteTransaction sqlTransaction = null;
            do
            {
                #region If failed to updated data, delete and retry
                if (resultRowsAffected == -1)
                {
                    Logger.Error("Delete MediaExiftoolTagsWarning data due to previous application crash for file: " + exifToolNewValue.FullFilePath);

                    try
                    {
                        dbTools.TransactionRollback(sqlTransaction);
                    }
                    catch { }

                    //Delete all extries due to crash.
                    DeleteFileEntryFromMediaExiftoolTagsWarning(new FileEntry(exifToolNewValue.FileDirectory, exifToolNewValue.FileName, exifToolNewValue.FileDateModified));
                }
                #endregion

                sqlTransaction = dbTools.TransactionBegin();

                #region SELECT Warning FROM MediaExiftoolTagsWarning
                warning = "(Logged: " + DateTime.Now.ToString() + ")\r\n" + warning;
                string sqlRead =
                    "SELECT Warning FROM MediaExiftoolTagsWarning WHERE " +
                    "FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified AND " +
                    "OldRegion = @OldRegion AND OldCommand = @OldCommand AND " +
                    "NewRegion = @NewRegion AND NewCommand = @NewCommand";

                bool oldRecordFound = false;
                using (var commandDatabase = new CommonSqliteCommand(sqlRead, dbTools.ConnectionDatabase, sqlTransaction))
                {
                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", exifToolNewValue.FileDirectory);
                    commandDatabase.Parameters.AddWithValue("@FileName", exifToolNewValue.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(exifToolNewValue.FileDateModified));
                    commandDatabase.Parameters.AddWithValue("@OldRegion", exifToolOldValue.Region);
                    commandDatabase.Parameters.AddWithValue("@OldCommand", exifToolOldValue.Command);
                    commandDatabase.Parameters.AddWithValue("@NewRegion", exifToolNewValue.Region);
                    commandDatabase.Parameters.AddWithValue("@NewCommand", exifToolNewValue.Command);

                    using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            warning = warning + "\r\n" + dbTools.ConvertFromDBValString(reader["Warning"]);
                        }
                    }
                    oldRecordFound = true;
                }
                #endregion

                #region DELETE FROM MediaExiftoolTagsWarning
                if (oldRecordFound)
                {
                    string sqlDelete =
                    "DELETE FROM MediaExiftoolTagsWarning WHERE " +
                    "FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified AND " +
                    "OldRegion = @OldRegion AND OldCommand = @OldCommand AND " +
                    "NewRegion = @NewRegion AND NewCommand = @NewCommand";

                    using (var commandDatabase = new CommonSqliteCommand(sqlDelete, dbTools.ConnectionDatabase, sqlTransaction))
                    {
                        //commandDatabase.Prepare();
                        commandDatabase.Parameters.AddWithValue("@FileDirectory", exifToolNewValue.FileDirectory);
                        commandDatabase.Parameters.AddWithValue("@FileName", exifToolNewValue.FileName);
                        commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(exifToolNewValue.FileDateModified));
                        commandDatabase.Parameters.AddWithValue("@OldRegion", exifToolOldValue.Region);
                        commandDatabase.Parameters.AddWithValue("@OldCommand", exifToolOldValue.Command);
                        commandDatabase.Parameters.AddWithValue("@NewRegion", exifToolNewValue.Region);
                        commandDatabase.Parameters.AddWithValue("@NewCommand", exifToolNewValue.Command);
                        resultRowsAffected = commandDatabase.ExecuteNonQuery();
                    }
                }
                #endregion

                #region INSERT INTO MediaExiftoolTagsWarning 
                if (resultRowsAffected != -1)
                {
                    string sqlCommand =
                    "INSERT INTO MediaExiftoolTagsWarning (FileDirectory, FileName, FileDateModified, OldRegion, OldCommand, OldParameter, NewRegion, NewCommand, NewParameter, Warning) " +
                    "Values (@FileDirectory, @FileName, @FileDateModified, @OldRegion, @OldCommand, @OldParameter, @NewRegion, @NewCommand, @NewParameter, @Warning)";

                    using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
                    {
                        //commandDatabase.Prepare();
                        commandDatabase.Parameters.AddWithValue("@FileDirectory", exifToolNewValue.FileDirectory);
                        commandDatabase.Parameters.AddWithValue("@FileName", exifToolNewValue.FileName);
                        commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(exifToolNewValue.FileDateModified));
                        commandDatabase.Parameters.AddWithValue("@OldRegion", exifToolOldValue.Region);
                        commandDatabase.Parameters.AddWithValue("@OldCommand", exifToolOldValue.Command);
                        commandDatabase.Parameters.AddWithValue("@OldParameter", exifToolOldValue.Parameter);
                        commandDatabase.Parameters.AddWithValue("@NewRegion", exifToolNewValue.Region);
                        commandDatabase.Parameters.AddWithValue("@NewCommand", exifToolNewValue.Command);
                        commandDatabase.Parameters.AddWithValue("@NewParameter", exifToolNewValue.Parameter);
                        commandDatabase.Parameters.AddWithValue("@Warning", warning);

                        resultRowsAffected = commandDatabase.ExecuteNonQuery();


                    }
                }
                #endregion

            } while (resultRowsAffected == -1 || !dbTools.TransactionCommit(sqlTransaction));
        }
        #endregion

        #region DeleteDirectoryAndHistory
        public int DeleteDirectoryAndHistory(string fileDirectory)
        {
            int rowsAffected = 0;

            Mono.Data.Sqlite.SqliteTransaction sqlTransaction;
            do
            {
                sqlTransaction = dbTools.TransactionBegin();

                #region DELETE FROM MediaExiftoolTagsWarning 
                string sqlCommand = "DELETE FROM MediaExiftoolTagsWarning WHERE FileDirectory = @FileDirectory";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
                {
                    //commandDatabase.Prepare();
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                    rowsAffected = commandDatabase.ExecuteNonQuery();      // Execute the query
                }
                #endregion

            } while (!dbTools.TransactionCommit(sqlTransaction));

            return rowsAffected;
        }
        #endregion

        #region DeleteFileEntryFromMediaExiftoolTagsWarning
        private void DeleteFileEntryFromMediaExiftoolTagsWarning(FileEntry fileEntry)
        {
            List<FileEntry> fileEntries = new List<FileEntry>();
            fileEntries.Add(fileEntry);
            DeleteFileEntriesFromMediaExiftoolTagsWarning(fileEntries);
        }
        #endregion

        #region DeleteFileEntriesFromMediaExiftoolTagsWarning
        public void DeleteFileEntriesFromMediaExiftoolTagsWarning(List<FileEntry> fileEntries)
        {
            Mono.Data.Sqlite.SqliteTransaction sqlTransaction;
            do
            {
                sqlTransaction = dbTools.TransactionBegin();

                #region DELETE FROM MediaExiftoolTagsWarning 
                string sqlCommand = "DELETE FROM MediaExiftoolTagsWarning " +
                    "WHERE FileDirectory = @FileDirectory " +
                    "AND FileName = @FileName " +
                    "AND FileDateModified = @FileDateModified ";
                using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
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
            
            } while (!dbTools.TransactionCommit(sqlTransaction));
        }
        #endregion

        #region ListFileEntryDateVersions
        public List<FileEntryAttribute> ListFileEntryDateVersions(string fullFileName)
        {
            return ListFileEntryDateVersions(Path.GetDirectoryName(fullFileName), Path.GetFileName(fullFileName));
        }
        #endregion

        #region ListFileEntryDateVersions
        public List<FileEntryAttribute> ListFileEntryDateVersions(string fileDirectory, string fileName)
        {
            List<FileEntryAttribute> exifToolDates = new List<FileEntryAttribute>();

            Mono.Data.Sqlite.SqliteTransaction sqlTransactionSelect;
            do
            {
                sqlTransactionSelect = dbTools.TransactionBeginSelect();

                #region SELECT DISTINCT FileDirectory, FileName, FileDateModified FROM MediaExiftoolTagsWarning 
                string sqlCommand = "SELECT DISTINCT FileDirectory, FileName, FileDateModified FROM MediaExiftoolTagsWarning " + 
                "WHERE FileDirectory = @FileDirectory AND FileName = @FileName";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
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

                        FileEntryAttribute fileEntryAttribute = new FileEntryAttribute
                            (
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            currentMetadataDate, 
                            FileEntryVersion.Historical
                            );
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

            } while (!dbTools.TransactionCommitSelect(sqlTransactionSelect));

            return exifToolDates;
        }
        #endregion

    }
}
