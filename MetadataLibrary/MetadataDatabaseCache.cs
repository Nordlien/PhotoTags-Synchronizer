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
using System.Drawing;
using System.IO;


namespace MetadataLibrary
{
    public class MetadataDatabaseCache
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private SqliteDatabaseUtilities dbTools;
        public MetadataDatabaseCache(SqliteDatabaseUtilities databaseTools)
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


        #region Tables: Metadata Tag and Region
        private Metadata Read(FileEntryBroker file)
        {
            Metadata metadata = new Metadata(file.Broker);

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
                commandDatabase.Parameters.AddWithValue("@Broker", (int)file.Broker);
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(file.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(file.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(file.LastWriteDateTime));

                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        metadata.Broker = (MetadataBrokerTypes)dbTools.ConvertFromDBValLong(reader["Broker"]);
                        metadata.FileDirectory = dbTools.ConvertFromDBValString(reader["FileDirectory"]);
                        metadata.FileName = dbTools.ConvertFromDBValString(reader["FileName"]);
                        metadata.FileSize = dbTools.ConvertFromDBValLong(reader["FileSize"]);
                        metadata.FileDateCreated = dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateCreated"]);
                        metadata.FileDateModified = dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]);
                        metadata.FileLastAccessed = dbTools.ConvertFromDBValDateTimeLocal(reader["FileLastAccessed"]);
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
                    }
                    else
                    {
                        return null; //No data found, return null
                    }
                }
            }

             sqlCommand =
                    "SELECT " +
                        "Broker, FileDirectory, FileName, FileDateModified, Keyword, Confidence " +
                    "FROM MediaPersonalKeywords WHERE (Broker & @Broker) = @Broker AND FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)file.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(file.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(file.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(file.LastWriteDateTime));
                commandDatabase.Prepare();

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

            sqlCommand =
                    "SELECT " +
                    "Broker, FileDirectory, FileName, FileDateModified, Type, " +
                    "Name, AreaX, AreaY, AreaWidth, AreaHeight, RegionStructureType, Thumbnail " +
                    "FROM MediaPersonalRegions WHERE (Broker & @Broker) = @Broker AND FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)file.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(file.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(file.FullFilePath));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(file.LastWriteDateTime));

                commandDatabase.Prepare();

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
            return metadata;
        }

        public void Write(Metadata metadata)
        {
            if (metadata == null) throw new Exception("Error in DatabaseCache. metaData is Null. Error in code");

            dbTools.TransactionBeginBatch();

            CacheUpdate(metadata);

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
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadata.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", metadata.FileDirectory);
                commandDatabase.Parameters.AddWithValue("@FileName", metadata.FileName);
                commandDatabase.Parameters.AddWithValue("@FileSize", metadata.FileSize);
                commandDatabase.Parameters.AddWithValue("@FileDateCreated", dbTools.ConvertFromDateTimeToDBVal(metadata.FileDateCreated));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(metadata.FileDateModified));
                commandDatabase.Parameters.AddWithValue("@FileLastAccessed", dbTools.ConvertFromDateTimeToDBVal(metadata.FileLastAccessed));
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

                commandDatabase.Prepare();
                //commandDatabase.ExecuteNonQuery();
                if (commandDatabase.ExecuteNonQuery() == -1)
                {
                    Logger.Error("Delete MediaMetadata and sub data due to previous application crash for file: " + metadata.FileFullPath);
                    //Delete all extries due to crash.
                    DeleteFileEntry(metadata.FileEntryBroker);
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
                foreach (KeywordTag tag in metadata.PersonalKeywordTags)
                {
                    commandDatabase.Parameters.AddWithValue("@Broker", metadata.Broker);
                    commandDatabase.Parameters.AddWithValue("@FileDirectory", metadata.FileDirectory);
                    commandDatabase.Parameters.AddWithValue("@FileName", metadata.FileName);
                    commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(metadata.FileDateModified));
                    commandDatabase.Parameters.AddWithValue("@Keyword", tag.Keyword);
                    commandDatabase.Parameters.AddWithValue("@Confidence", tag.Confidence);
                    commandDatabase.Prepare();

                    if (commandDatabase.ExecuteNonQuery() == -1)
                    {
                        Logger.Error("Delete MediaPersonalKeywords data due to previous application crash for file: " + metadata.FileFullPath);
                        //Delete all extries due to crash.
                        DeleteFileMediaPersonalKeywords(metadata.FileEntryBroker);
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
                foreach (RegionStructure region in metadata.PersonalRegionList)
                {
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
                    commandDatabase.Prepare();
                    
                    if (commandDatabase.ExecuteNonQuery() == -1)
                    {
                        Logger.Error("Delete MediaPersonalRegions data due to previous application crash for file: " + metadata.FileFullPath);
                        //Delete all extries due to crash.
                        DeleteFileMediaPersonalRegions(metadata.FileEntryBroker);
                        commandDatabase.ExecuteNonQuery();
                    }  
                }
            }

            dbTools.TransactionCommitBatch();
        }

        public void Copy(string oldDirectory, string oldFilename, string newDirectory, string newFilename)
        {
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
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
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
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
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
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand = 
                "INSERT INTO MediaExiftoolTags (FileDirectory, FileName, FileDateModified, Region, Command, Parameter) " +
                "SELECT @NewFileDirectory, @NewFileName, FileDateModified, Region, Command, Parameter FROM " +
                "MediaExiftoolTags WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand =
                "INSERT INTO MediaExiftoolTagsWarning " + 
                    "(FileDirectory, FileName, FileDateModified, OldRegion, OldCommand, OldParameter, NewRegion, NewCommand, NewParameter, Warning) " +
                "SELECT @NewFileDirectory, @NewFileName, FileDateModified, OldRegion, OldCommand, OldParameter, NewRegion, NewCommand, NewParameter, Warning FROM " +
                    "MediaExiftoolTagsWarning WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }
            dbTools.TransactionCommitBatch();

            sqlCommand =
                "INSERT INTO MediaThumbnail (FileDirectory, FileName, FileDateModified, Image) " +
                "SELECT @NewFileDirectory, @NewFileName, FileDateModified, Image FROM MediaThumbnail WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }
            dbTools.TransactionCommitBatch();
        }

        public void Move(string oldDirectory, string oldFilename, string newDirectory, string newFilename)
        {      
            dbTools.TransactionBeginBatch();

            ClearCache();

            string sqlCommand =
                "UPDATE MediaMetadata SET " + 
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand =
                "UPDATE MediaPersonalKeywords SET " + 
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand =
                "UPDATE MediaPersonalRegions SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand =
                "UPDATE MediaExiftoolTags SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }

            sqlCommand =
                "UPDATE MediaExiftoolTagsWarning SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }
            dbTools.TransactionCommitBatch();

            sqlCommand =
                "UPDATE MediaThumbnail SET " +
                "FileDirectory = @NewFileDirectory, FileName = @NewFileName " +
                "WHERE FileDirectory = @OldFileDirectory AND FileName = @OldFileName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@OldFileName", oldFilename);
                commandDatabase.Parameters.AddWithValue("@OldFileDirectory", oldDirectory);
                commandDatabase.Parameters.AddWithValue("@NewFileName", newFilename);
                commandDatabase.Parameters.AddWithValue("@NewFileDirectory", newDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
            }
            dbTools.TransactionCommitBatch();
        }

        public void UpdateRegionThumbnail(FileEntryBroker file, RegionStructure region)
        {
            CacheRemove(file);

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

        public Image ReadRegionThumbnail(FileEntryBroker file, RegionStructure region)
        {
            Image image = null;

            string sqlCommand =
                    "SELECT Thumbnail FROM MediaPersonalRegions " +
                    "WHERE Broker = @Broker " +
                    "AND FileDirectory = @FileDirectory " +
                    "AND FileName = @FileName " +
                    "AND FileDateModified = @FileDateModified " +
                    "AND Type = @Type " +
                    "AND Name = @Name " +
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

                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();

                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        image = dbTools.ByteArrayToImage(dbTools.ConvertFromDBValByteArray(reader["Thumbnail"]));
                    }
                }
            }
            return image;
        }

        private void DeleteDirectoryMediaMetadata(MetadataBrokerTypes broker, string fileDirectory)
        {
            string sqlCommand = "DELETE FROM MediaMetadata WHERE " +
                            "Broker = @Broker AND " +
                            "FileDirectory = @FileDirectory";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        private void DeleteDirectoryMediaPersonalRegions(MetadataBrokerTypes broker, string fileDirectory)
        {
            string sqlCommand = "DELETE FROM MediaPersonalRegions WHERE " +
                            "Broker = @Broker AND " +
                            "FileDirectory = @FileDirectory";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        private void DeleteDirectoryMediaPersonalKeywords(MetadataBrokerTypes broker, string fileDirectory)
        {
            string sqlCommand = "DELETE FROM MediaPersonalKeywords WHERE " +
                            "Broker = @Broker AND " +
                            "FileDirectory = @FileDirectory";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileDirectory);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        public void DeleteDirectory(MetadataBrokerTypes broker, string fileDirectory)
        {
            ClearCache();

            DeleteDirectoryMediaMetadata(broker, fileDirectory);
            DeleteDirectoryMediaPersonalRegions(broker, fileDirectory);
            DeleteDirectoryMediaPersonalKeywords(broker, fileDirectory);
        }
        
        private void DeleteFileMediaMetadata(FileEntryBroker fileEntryBroker)
        {
            string sqlCommand = "DELETE FROM MediaMetadata WHERE " +
                            "Broker = @Broker " +
                            "AND FileDirectory = @FileDirectory " +
                            "AND FileName = @FileName " +
                            "AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", fileEntryBroker.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntryBroker.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntryBroker.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntryBroker.LastWriteDateTime));
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        private void DeleteFileMediaPersonalRegions(FileEntryBroker fileEntryBroker)
        {
            string sqlCommand = "DELETE FROM MediaPersonalRegions WHERE " +
                "Broker = @Broker " +
                "AND FileDirectory = @FileDirectory " +
                "AND FileName = @FileName " +
                "AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", fileEntryBroker.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntryBroker.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntryBroker.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntryBroker.LastWriteDateTime));
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        private void DeleteFileMediaPersonalKeywords(FileEntryBroker fileEntryBroker)
        {
            string sqlCommand = "DELETE FROM MediaPersonalKeywords WHERE " +
                            "Broker = @Broker " +
                            "AND FileDirectory = @FileDirectory " +
                            "AND FileName = @FileName " +
                            "AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", fileEntryBroker.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", fileEntryBroker.Directory);
                commandDatabase.Parameters.AddWithValue("@FileName", fileEntryBroker.FileName);
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(fileEntryBroker.LastWriteDateTime));
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        public void DeleteFileEntry(FileEntryBroker fileEntryBroker)
        {
            CacheRemove(fileEntryBroker);

            DeleteFileMediaMetadata(fileEntryBroker);
            DeleteFileMediaPersonalRegions(fileEntryBroker);
            DeleteFileMediaPersonalKeywords(fileEntryBroker);

        }

        public List<FileEntryBroker> ListFileEntryDateVersions(MetadataBrokerTypes broker, string fullFileName)
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
                commandDatabase.Parameters.AddWithValue("@Broker", (int)broker);
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(fullFileName));
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(fullFileName));
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FileEntryBroker fileEntryBroker = new FileEntryBroker
                            (
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]),
                            (MetadataBrokerTypes)dbTools.ConvertFromDBValLong(reader["Broker"])
                            );
                        fileEntryBrokers.Add(fileEntryBroker);
                    }
                }
            }

            return fileEntryBrokers;
        }

        public List<String> ListAllMissingFileEntries(MetadataBrokerTypes broker, List<FileEntry> files)
        {
            if (files == null) return null;
            
            List<String> mediaFilesNoInDatabase = new List<String>();


            string sqlCommand = "SELECT 1 FROM MediaMetadata WHERE " +
                "Broker = @Broker AND " +
                "FileDirectory = @FileDirectory AND " +
                "FileName = @FileName AND " +
                "FileDateModified = @FileDateModified " +
                "LIMIT 1";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                foreach (FileEntry file in files)
                {
                    FileEntryBroker fileEntryBroker = new FileEntryBroker(file.FullFilePath, file.LastWriteDateTime, broker);

                    if (!CacheContainsKey(fileEntryBroker)) //Check if already in queue, due to screen refreash and reloads etc...
                    {
                        commandDatabase.Parameters.AddWithValue("@Broker", broker);
                        commandDatabase.Parameters.AddWithValue("@FileDirectory", file.Directory);
                        commandDatabase.Parameters.AddWithValue("@FileName", file.FileName);
                        commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(file.LastWriteDateTime));
                        var value = commandDatabase.ExecuteScalar();

                        if (value == null)
                        {
                            mediaFilesNoInDatabase.Add(fileEntryBroker.FullFilePath);
                        }
                    }
                }
            }

            return mediaFilesNoInDatabase;
        }

        #endregion


        #region Cache
        Dictionary<FileEntryBroker, Metadata> metadataCache = new Dictionary<FileEntryBroker, Metadata>();

        public SqliteDatabaseUtilities DbTools { get => dbTools; set => dbTools = value; }

        private void CacheUpdate(Metadata metadata)
        {
            if (metadata.FileName == null)
            {
                return;
            }
            FileEntryBroker file = new FileEntryBroker(
                Path.Combine(metadata.FileDirectory, metadata.FileName),
                (DateTime)metadata.FileDateModified,
                metadata.Broker);
            //Update cache
            if (metadataCache.ContainsKey(file))
            {
                metadataCache[file] = metadata;
            }
            else
            {
                metadataCache.Add(file, metadata);
            }
        }

        private bool CacheContainsKey(FileEntryBroker file)
        {
            return metadataCache.ContainsKey(file);
        }

        private Metadata CacheGet(FileEntryBroker file)
        {
            return metadataCache[file];
        }

        public void CacheRemove(FileEntryBroker file)
        {
            if (file == null) return;
            if (CacheContainsKey(file))
            {
                metadataCache.Remove(file);
            }
        }

        public void CacheRemove(FileEntryBroker[] files)
        {
            if (files == null) return;
            foreach (FileEntryBroker file in files)
            {
                if (CacheContainsKey(file))
                {
                    metadataCache.Remove(file);
                }
            }
        }

        public void ClearCache()
        {
            metadataCache = null;
            metadataCache = new Dictionary<FileEntryBroker, Metadata>();            
        }

        

        public Metadata ReadCache(FileEntryBroker file)
        {
            if (CacheContainsKey(file))
            {
                return CacheGet(file);
            }

            Metadata metadata;

            metadata = Read(file);
            if (metadata != null)
            {
                CacheUpdate(metadata);
                return metadata;
            }
            else
            {
                return null;
            }
        }
        #endregion

    }
}
