#define MonoSqlite
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


        #region Table: MediaExiftoolTagsWarning
        public List<ExiftoolWarningData> Read(FileEntry fileEntry)
        {
            
            List<ExiftoolWarningData> exifToolDataList = new List<ExiftoolWarningData>();

            string sqlCommand = "SELECT FileDirectory, FileName, FileDateModified, OldRegion, OldCommand, OldParameter, NewRegion, NewCommand, NewParameter, Warning FROM " +
                "MediaExiftoolTagsWarning WHERE FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified=@FileDateModified";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fileEntry.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fileEntry.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntry.LastWriteDateTime)); 
                commandDatabase.Prepare();

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
                            dbTools.ConvertFromDBValString(reader["OldParameter"]));
                        exifToolWarningData.NewExiftoolData = new ExiftoolData(
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]),
                            dbTools.ConvertFromDBValString(reader["NewRegion"]),
                            dbTools.ConvertFromDBValString(reader["NewCommand"]),
                            dbTools.ConvertFromDBValString(reader["NewParameter"]));
                        exifToolWarningData.WarningMessage = dbTools.ConvertFromDBValString(reader["Warning"]);
                        exifToolDataList.Add(exifToolWarningData);
                    }
                }
            }
            return exifToolDataList;
        }

        public void Write(ExiftoolData exifToolOldValue, ExiftoolData exifToolNewValue, string warning)
        {
            dbTools.TransactionBeginBatch();

            string sqlCommand =
                "INSERT INTO MediaExiftoolTagsWarning (FileDirectory, FileName, FileDateModified, OldRegion, OldCommand, OldParameter, NewRegion, NewCommand, NewParameter, Warning) " +
                "Values (@FileDirectory, @FileName, @FileDateModified, @OldRegion, @OldCommand, @OldParameter, @NewRegion, @NewCommand, @NewParameter, @Warning)";

            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
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
                commandDatabase.Prepare();
                //commandDatabase.ExecuteNonQuery();      // Execute the query
                if (commandDatabase.ExecuteNonQuery() == -1)
                {
                    Logger.Error("Delete MediaExiftoolTagsWarning data due to previous application crash for file: " + exifToolNewValue.FullFilePath);
                    //Delete all extries due to crash.
                    DeleteFileEntry(new FileEntry (exifToolNewValue.FileDirectory, exifToolNewValue.FileName, exifToolNewValue.FileDateModified));
                    commandDatabase.ExecuteNonQuery();
                }
            }

            dbTools.TransactionCommitBatch();
        }

        public void DeleteDirectory(string fileDirectory)
        {
            string sqlCommand = "DELETE FROM MediaExiftoolTagsWarning WHERE FileDirectory = @FileDirectory";
            using (var commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        public void DeleteFileEntry(FileEntry fileEntry)
        {
            string sqlCommand = "DELETE FROM MediaExiftoolTagsWarning " +
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

        
        public List<FileEntry> ListFileEntryDateVersions(string fullFileName)
        {
            return ListFileEntryDateVersions(Path.GetDirectoryName(fullFileName), Path.GetFileName(fullFileName));
        }

        public List<FileEntry> ListFileEntryDateVersions(string fileDirectory, string fileName)
        {
            List<FileEntry> exifToolDates = new List<FileEntry>();

            string sqlCommand = "SELECT DISTINCT FileDirectory, FileName, FileDateModified FROM MediaExiftoolTagsWarning " + 
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
                        exifToolDates.Add(fileEntry);
                    }
                }

                exifToolDates.Sort();
            }
            return exifToolDates;
        }

        #endregion

    }
}
