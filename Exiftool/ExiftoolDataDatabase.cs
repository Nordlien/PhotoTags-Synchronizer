#define MonoSqlite
#if MonoSqlite
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

using System;
using System.Collections.Generic;
using System.IO;
using MetadataLibrary;
using SqliteDatabase;
using System.Diagnostics;
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

        public void TransactionBeginBatch()
        {
            dbTools.TransactionBeginBatch();
        }

        public void TransactionCommitBatch()
        {
            dbTools.TransactionCommitBatch();
        }

        #region Table: MediaExiftoolTags

        public List<ExiftoolData> Read(FileEntry file)
        {
            List<ExiftoolData> exifToolDataList = new List<ExiftoolData>();

            string sqlCommand = "SELECT FileDirectory, FileName, FileDateModified, Region, Command, Parameter FROM " +
                "MediaExiftoolTags WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(file.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(file.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(file.LastWriteDateTime));
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ExiftoolData exifToolData = new ExiftoolData();
                        exifToolData.FileDirectory = dbTools.ConvertFromDBValString(reader["FileDirectory"]);
                        exifToolData.FileName = dbTools.ConvertFromDBValString(reader["FileName"]);
                        exifToolData.FileDateModified = (DateTime)dbTools.ConvertFromDBValDateTime(reader["FileDateModified"]);
                        exifToolData.Region = dbTools.ConvertFromDBValString(reader["Region"]);
                        exifToolData.Command = dbTools.ConvertFromDBValString(reader["Command"]);
                        exifToolData.Parameter = dbTools.ConvertFromDBValString(reader["Parameter"]);
                        exifToolDataList.Add(exifToolData);
                    }
                }
            }
            return exifToolDataList;
        }

        public bool Write(ExiftoolData exifToolData)
        {
            string sqlCommand =
            "INSERT INTO MediaExiftoolTags (FileDirectory, FileName, FileDateModified, Region, Command, Parameter) " +
            "Values (@FileDirectory, @FileName, @FileDateModified, @Region, @Command, @Parameter)";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", exifToolData.FileDirectory);
                commandDatabase.Parameters.AddWithValue("@FileName", exifToolData.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(exifToolData.FileDateModified));
                commandDatabase.Parameters.AddWithValue("@Region", exifToolData.Region);
                commandDatabase.Parameters.AddWithValue("@Command", exifToolData.Command);
                commandDatabase.Parameters.AddWithValue("@Parameter", exifToolData.Parameter);
                commandDatabase.Prepare();
                
                if (commandDatabase.ExecuteNonQuery() == -1)
                {
                    Logger.Error("Delete MediaExiftoolTags data due to previous application crash for file: " + exifToolData.FullFilePath);
                    //Delete all extries due to crash.
                    DeleteFileMediaExiftoolTags(new FileEntry(exifToolData.FileDirectory, exifToolData.FileName, exifToolData.FileDateModified));
                    commandDatabase.ExecuteNonQuery();
                    return false;                     
                }   // Execute the query
                return true;
            }
        }

        public void DeleteDirectory(string fileDirectory)
        {
            string sqlCommand = "DELETE FROM MediaExiftoolTags WHERE FileDirectory = @FileDirectory";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        public void DeleteFileMediaExiftoolTags(FileEntry fileEntry)
        {
            string sqlCommand = "DELETE FROM MediaExiftoolTags " +
                "WHERE FileDirectory = @FileDirectory " +
                "AND FileName = @FileName " +
                "AND FileDateModified = @FileDateModified ";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntry.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntry.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime));
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }


        public List<FileEntry> ListFileEntryDateVersions(string fileName)
        {
            return ListFileEntryDateVersions(Path.GetFileName(fileName), Path.GetDirectoryName(fileName));
        }

        public List<FileEntry> ListFileEntryDateVersions(string fileName, string fileDirectory)
        {
            List<FileEntry> exifToolDates = new List<FileEntry>();
            string sqlCommand = "SELECT DISTINCT FileDirectory, FileName, FileDateModified FROM MediaExiftoolTags " + 
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
                        FileEntry fileEntry = new FileEntry(
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTime(reader["FileDateModified"]));
                        exifToolDates.Add(fileEntry);
                        Logger.Trace(fileEntry.FullFilePath + " " + fileEntry.LastWriteDateTime);
                    }
                }

                exifToolDates.Sort();
            }
            return exifToolDates;
        }


        #endregion 

    }
}
