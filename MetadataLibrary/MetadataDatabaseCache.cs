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
using System.IO;
using System.Linq;

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

        #region Read
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
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(file.FileFullPath));
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(file.FileFullPath));
                commandDatabase.Parameters.AddWithValue("@FileDateModified", dbTools.ConvertFromDateTimeToDBVal(file.LastWriteDateTime));

                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        metadata.Broker = (MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"]);
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
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(file.FileFullPath));
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(file.FileFullPath));
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
                    "FROM MediaPersonalRegions " +
                    "WHERE (Broker & @Broker) = @Broker AND FileDirectory = @FileDirectory AND FileName = @FileName AND FileDateModified = @FileDateModified";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)file.Broker);
                commandDatabase.Parameters.AddWithValue("@FileDirectory", Path.GetDirectoryName(file.FileFullPath));
                commandDatabase.Parameters.AddWithValue("@FileName", Path.GetFileName(file.FileFullPath));
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
        #endregion

        #region Write
        public void Write(Metadata metadata)
        {
            if (metadata == null) throw new Exception("Error in DatabaseCache. metaData is Null. Error in code");

            CacheRemove(metadata.FileEntryBroker); 
            dbTools.TransactionBeginBatch();

            MetadataCacheUpdate(metadata.FileEntryBroker, metadata);

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
        #endregion

        #region Copy
        public void Copy(string oldDirectory, string oldFilename, string newDirectory, string newFilename)
        {
            ClearCache();
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
        #endregion

        #region Move
        public void Move(string oldDirectory, string oldFilename, string newDirectory, string newFilename)
        {
            ClearCache();
            dbTools.TransactionBeginBatch();

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
        #endregion

        #region UpdateRegionThumbnail
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
        #endregion

        #region Delete Directoy - Mediadata
        private void DeleteDirectoryMediaMetadata(MetadataBrokerType broker, string fileDirectory)
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
        #endregion

        #region Delete Directory - Media PersonalRegions
        private void DeleteDirectoryMediaPersonalRegions(MetadataBrokerType broker, string fileDirectory)
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
        #endregion

        #region Delete Directory - Media PersonalKeywords 
        private void DeleteDirectoryMediaPersonalKeywords(MetadataBrokerType broker, string fileDirectory)
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
        #endregion

        #region Delete Directory
        public void DeleteDirectory(MetadataBrokerType broker, string fileDirectory)
        {
            ClearCache();

            DeleteDirectoryMediaMetadata(broker, fileDirectory);
            DeleteDirectoryMediaPersonalRegions(broker, fileDirectory);
            DeleteDirectoryMediaPersonalKeywords(broker, fileDirectory);
        }
        #endregion

        #region Delete File - Metadata
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
        #endregion

        #region Delete File - Personal Regions
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
        #endregion

        #region Delete File - Personal Keywords
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
        #endregion

        #region Delete File
        public void DeleteFileEntry(FileEntryBroker fileEntryBroker)
        {
            CacheRemove(fileEntryBroker);

            DeleteFileMediaMetadata(fileEntryBroker);
            DeleteFileMediaPersonalRegions(fileEntryBroker);
            DeleteFileMediaPersonalKeywords(fileEntryBroker);

        }
        #endregion

        #region List File Date Versions

        #region List File Date Versions - Broker
        public List<FileEntryBroker> ListFileEntryBrokerDateVersions(MetadataBrokerType broker, string fullFileName)
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
                            (MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"])
                            );
                        fileEntryBrokers.Add(fileEntryBroker);
                    }
                }
            }

            return fileEntryBrokers;
        }
        #endregion 

        Dictionary<FileBroker, List<FileEntryAttribute>> listFileAttributeDateVersions = new Dictionary<FileBroker, List<FileEntryAttribute>>();

        #region List File Date Versions - Attribute
        public List<FileEntryAttribute> ListFileEntryAttributesCache(MetadataBrokerType broker, string fullFileName)
        {
            FileBroker fileBroker = new FileBroker(broker, fullFileName);
            if (listFileAttributeDateVersions.ContainsKey(fileBroker)) return listFileAttributeDateVersions[fileBroker];

            List<FileEntryAttribute> fileEntryAttributes = new List<FileEntryAttribute>();
            ListFileEntryAttributes2(ref fileEntryAttributes, broker, fullFileName);
            MetadataBrokerType broker2 = broker | MetadataBrokerType.ExifToolWriteError;
            ListFileEntryAttributes2(ref fileEntryAttributes, broker2, fullFileName);

            listFileAttributeDateVersions.Add(fileBroker, fileEntryAttributes);

            return fileEntryAttributes;
        }

        private void ListFileEntryAttributesCacheRemove(FileBroker fileBroker)
        {
            if (listFileAttributeDateVersions.ContainsKey(fileBroker)) listFileAttributeDateVersions.Remove(fileBroker);
        }

        private void ListFileEntryAttributesCacheClear()
        {
            listFileAttributeDateVersions = new Dictionary<FileBroker, List<FileEntryAttribute>>();
        }

        private void ListFileEntryAttributes2(ref List<FileEntryAttribute> FileEntryAttributes, MetadataBrokerType broker, string fullFileName)
        {
            
            string sqlCommand =
                "SELECT " +
                    "Broker, FileDirectory, FileName, FileDateModified " +
                    "FROM MediaMetadata WHERE " +
                    "Broker = @Broker AND " +
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
                    FileEntryAttribute newstFileEntryAttributeForEdit = null;

                    while (reader.Read())
                    {
                        bool isErrorVersion = false;
                        DateTime currentMetadataDate = (DateTime)dbTools.ConvertFromDBValDateTimeLocal(reader["FileDateModified"]);
                        if (((MetadataBrokerType)dbTools.ConvertFromDBValLong(reader["Broker"]) & MetadataBrokerType.ExifToolWriteError) == MetadataBrokerType.ExifToolWriteError) 
                            isErrorVersion = true;
                        FileEntryAttribute fileEntryAttribute = new FileEntryAttribute
                            (
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"]),
                            currentMetadataDate,
                            isErrorVersion ? FileEntryVersion.Error : FileEntryVersion.Historical
                            );
                        FileEntryAttributes.Add(fileEntryAttribute);

                        if (!isErrorVersion && (newstFileEntryAttributeForEdit == null || currentMetadataDate > newstFileEntryAttributeForEdit.LastWriteDateTime))
                        {
                            newstFileEntryAttributeForEdit = new FileEntryAttribute((FileEntry)fileEntryAttribute, FileEntryVersion.Current);
                        }
                    }
                    if (newstFileEntryAttributeForEdit != null) FileEntryAttributes.Add(newstFileEntryAttributeForEdit);                    
                }
            }
            //return FileEntryAttributes;
        }
        #endregion 

        #endregion

        #region List Files - Missing Entries
        public List<String> ListAllMissingFileEntries(MetadataBrokerType broker, List<FileEntry> files)
        {
            if (files == null) return null;

            List<String> mediaFilesNoInDatabase = new List<String>();

            foreach (FileEntry file in files)
            {
                FileEntryBroker fileEntryBroker = new FileEntryBroker(file.FileFullPath, file.LastWriteDateTime, broker);

                if (!MetadataCacheContainsKey(fileEntryBroker)) //Check if already in queue, due to screen refreash and reloads etc...
                {
                    Metadata metadata = ReadMetadataFromCacheOrDatabase(fileEntryBroker);
                    if (metadata == null) mediaFilesNoInDatabase.Add(fileEntryBroker.FileFullPath);
                }
            }

            return mediaFilesNoInDatabase;
        }
        #endregion

        #region List files - Search
        public List<string> ListAllSearch(MetadataBrokerType broker, bool useAndBetweenGrups,
            bool useMediaTakenFrom, DateTime mediaTakenFrom, bool useMediaTakenTo, DateTime mediaTakenTo, bool isMediaTakenNull,
            bool useAndBetweenTextTags,
            bool usePersonalAlbum, string personalAlbum,
            bool usePersonalTitle, string personalTitle,
            bool usePersonalComments, string personalComments,
            bool usePersonalDescription, string personalDescription,
            bool isRatingNull, bool hasRating0, bool hasRating1, bool hasRating2, bool hasRating3, bool hasRating4, bool hasRating5,
            bool useLocationName, string locationName,
            bool useLocationCity, string locationCity,
            bool useLocationState, string locationState,
            bool useLocationCountry, string locationCountry,
            bool useRegionNameList, bool needAlRegionNames, List<string> regionNameList, bool withoutRegions,
            bool useKeywordList, bool needAllKeywords, List<string> keywords, bool withoutKeywords,
            bool checkIfHasExifWarning, int maxRowsInResult
            )
        {

            List<string> listing = new List<string>();

            string sqlCommandBasicSelect = "SELECT DISTINCT MediaMetadata.Broker, MediaMetadata.FileDirectory, MediaMetadata.FileName FROM MediaMetadata ";

            #region Warning
            if (checkIfHasExifWarning) sqlCommandBasicSelect +=
                "LEFT JOIN MediaExiftoolTagsWarning ON " +
                "MediaExiftoolTagsWarning.FileDirectory = MediaMetadata.FileDirectory AND " +
                "MediaExiftoolTagsWarning.FileName = MediaMetadata.FileName AND " +
                "MediaExiftoolTagsWarning.FileDateModified = MediaMetadata.FileDateModified ";
            #endregion

            #region Get only newest records, not historical
            sqlCommandBasicSelect += "WHERE MediaMetadata.Broker = @Broker AND MediaMetadata.FileDateModified = (SELECT MAX(MediaMetadataNewst.FileDateModified) FROM MediaMetadata AS MediaMetadataNewst " +
                "WHERE MediaMetadataNewst.Broker = MediaMetadata.Broker AND MediaMetadataNewst.FileDirectory = MediaMetadata.FileDirectory AND MediaMetadataNewst.FileName = MediaMetadata.FileName) ";
            #endregion

            #region DateTaken
            if (useMediaTakenFrom && useMediaTakenTo)
                sqlCommandBasicSelect += (useAndBetweenGrups ? "AND " : "OR ") + "((MediaDateTaken >= @MediaDateTakenFrom AND MediaDateTaken <= @MediaDateTakenTo) " + (isMediaTakenNull ? "OR MediaDateTaken IS NULL " : "") + ") ";
            else if (useMediaTakenFrom) sqlCommandBasicSelect += (useAndBetweenGrups ? "AND " : "OR ") + "(MediaDateTaken >= @MediaDateTakenFrom " + (isMediaTakenNull ? "OR MediaDateTaken IS NULL " : "") + ") ";
            else if (useMediaTakenTo) sqlCommandBasicSelect += (useAndBetweenGrups ? "AND " : "OR ") + "(MediaDateTaken <= @MediaDateTakenTo " + (isMediaTakenNull ? "OR MediaDateTaken IS NULL " : "") + ") ";
            else if (isMediaTakenNull) sqlCommandBasicSelect += (useAndBetweenGrups ? "AND " : "OR ") + "MediaDateTaken IS NULL ";
            #endregion

            string sqlCommand = "";

            #region Text field tags
            string sqlTextTags = "";
            if (usePersonalAlbum) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (personalAlbum == null ? "PersonalAlbum IS NULL " : "PersonalAlbum LIKE @PersonalAlbum ");
            if (usePersonalTitle) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (personalTitle == null ? "PersonalTitle IS NULL " : "PersonalTitle LIKE @PersonalTitle ");
            if (usePersonalComments) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (personalComments == null ? "PersonalComments IS NULL " : "PersonalComments LIKE @PersonalComments ");
            if (usePersonalDescription) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (personalDescription == null ? "PersonalDescription IS NULL " : "PersonalDescription LIKE @PersonalDescription ");
            if (useLocationName) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (locationName == null ? "LocationName IS NULL " : "LocationName LIKE @LocationName ");
            if (useLocationCity) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (locationCity == null ? "LocationCity IS NULL " : "LocationCity LIKE @LocationCity ");
            if (useLocationState) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (locationState == null ? "LocationState IS NULL " : "LocationState LIKE @LocationState ");
            if (useLocationCountry) sqlTextTags += (sqlTextTags == "" ? "" : useAndBetweenTextTags ? "AND " : "OR ") + (locationCountry == null ? "LocationCountry IS NULL " : "LocationCountry LIKE @LocationCountry ");
            if (sqlTextTags != "") sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + "(" + sqlTextTags + ") ";
            #endregion

            #region Rating
            string sqlRating = "";
            if (isRatingNull) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent IS NULL)";
            if (hasRating0) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent < 1)";
            if (hasRating1) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >=  1 AND PersonalRatingPercent <= 12)";
            if (hasRating2) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >  12 AND PersonalRatingPercent <= 37)";
            if (hasRating3) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >  37 AND PersonalRatingPercent <= 62)";
            if (hasRating4) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >  62 AND PersonalRatingPercent <= 87)";
            if (hasRating5) sqlRating += (sqlRating == "" ? "" : " OR ") + "(PersonalRatingPercent >  87)";
            if (sqlRating != "") sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + "(" + sqlRating + ") ";
            #endregion

            #region Warning 
            if (checkIfHasExifWarning) sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + "(MediaExiftoolTagsWarning.Warning IS NOT NULL) ";
            #endregion

            #region Region Names
            if ((useRegionNameList && regionNameList != null && regionNameList.Count > 0) || withoutRegions)
            {
                string sqlRegionsNotInList = "";
                if (withoutRegions)
                {
                    sqlRegionsNotInList =
                        "(SELECT 1 FROM MediaPersonalRegions WHERE MediaPersonalRegions.Broker = MediaMetadata.Broker AND " +
                        "MediaPersonalRegions.FileDirectory = MediaMetadata.FileDirectory AND " +
                        "MediaPersonalRegions.FileDateModified = MediaMetadata.FileDateModified AND " +
                        "MediaPersonalRegions.FileName = MediaMetadata.FileName) IS NULL ";
                }

                string sqlRegionNames = "";
                if ((useRegionNameList && regionNameList != null && regionNameList.Count > 0) || withoutRegions)
                {
                    for (int index = 0; index < regionNameList.Count; index++)
                    {
                        sqlRegionNames += (sqlRegionNames == "" ? "" : needAlRegionNames ? " AND " : " OR ") +
                            "(SELECT 1 FROM MediaPersonalRegions WHERE MediaPersonalRegions.Broker = MediaMetadata.Broker AND " +
                            "MediaPersonalRegions.FileDirectory = MediaMetadata.FileDirectory AND " +
                            "MediaPersonalRegions.FileName = MediaMetadata.FileName AND " +
                            "MediaPersonalRegions.FileDateModified = MediaMetadata.FileDateModified AND " +
                            "MediaPersonalRegions.Name " + (regionNameList[index] == null ? "IS NULL) = 1" : "LIKE @MediaPersonalRegionsName" + index.ToString() + ") = 1");
                    }
                }
                sqlRegionNames =
                    (sqlRegionsNotInList == "" ? "" : sqlRegionsNotInList) +
                    (sqlRegionsNotInList != "" && sqlRegionNames != "" ? " OR " : "") +
                    (sqlRegionNames == "" ? "" : " (" + sqlRegionNames + ")");

                sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + "(" + sqlRegionNames + ") ";
            }
            #endregion

            #region Keywords
            if ((useKeywordList && keywords != null && keywords.Count > 0) || withoutKeywords)
            {

                string sqlKeywordNotInList = "";
                if (withoutKeywords)
                {
                    sqlKeywordNotInList =
                        "(SELECT 1 FROM MediaPersonalKeywords WHERE MediaPersonalKeywords.Broker = MediaMetadata.Broker AND " +
                        "MediaPersonalKeywords.FileDirectory = MediaMetadata.FileDirectory AND " +
                        "MediaPersonalKeywords.FileDateModified = MediaMetadata.FileDateModified AND " +
                        "MediaPersonalKeywords.FileName = MediaMetadata.FileName) IS NULL ";
                }

                string sqlKeywordList = "";
                if (useKeywordList && keywords != null && keywords.Count > 0)
                {
                    for (int index = 0; index < keywords.Count; index++)
                    {
                        sqlKeywordList += (sqlKeywordList == "" ? "" : needAllKeywords ? " AND " : " OR ") +
                             "(SELECT 1 FROM MediaPersonalKeywords WHERE MediaPersonalKeywords.Broker = MediaMetadata.Broker AND " +
                             "MediaPersonalKeywords.FileDirectory = MediaMetadata.FileDirectory AND " +
                             "MediaPersonalKeywords.FileName = MediaMetadata.FileName AND " +
                             "MediaPersonalKeywords.FileDateModified = MediaMetadata.FileDateModified AND " +
                             "MediaPersonalKeywords.Keyword " + (keywords[index] == null ? "IS NULL) = 1" : "LIKE @MediaPersonalKeywordsKeyword" + index.ToString() + ") = 1");
                    }
                }

                sqlKeywordList =
                    (sqlKeywordNotInList == "" ? "" : sqlKeywordNotInList) +
                    (sqlKeywordNotInList != "" && sqlKeywordList != "" ? " OR " : "") +
                    (sqlKeywordList == "" ? "" : " (" + sqlKeywordList + ")");

                sqlCommand += (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND " : "OR ") + " (" + sqlKeywordList + ") ";
            }
            #endregion

            sqlCommand = sqlCommandBasicSelect + (sqlCommand == "" ? "" : useAndBetweenGrups ? "AND (" + sqlCommand + ") " : "OR (" + sqlCommand + ")");
            //sqlCommand = sqlCommandBasicSelect + (sqlCommand == "" ? "" : "AND (" + sqlCommand + ") ");  

            #region Limit
            sqlCommand += "LIMIT " + maxRowsInResult.ToString();
            #endregion 

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)broker);

                #region DateTaken
                if (useMediaTakenFrom) commandDatabase.Parameters.AddWithValue("@MediaDateTakenFrom", dbTools.ConvertFromDateTimeToDBVal(mediaTakenFrom));
                if (useMediaTakenTo) commandDatabase.Parameters.AddWithValue("@MediaDateTakenTo", dbTools.ConvertFromDateTimeToDBVal(mediaTakenTo));
                #endregion

                #region Text field tags
                if (usePersonalAlbum) commandDatabase.Parameters.AddWithValue("@PersonalAlbum", personalAlbum);
                if (usePersonalTitle) commandDatabase.Parameters.AddWithValue("@PersonalTitle", personalTitle);
                if (usePersonalComments) commandDatabase.Parameters.AddWithValue("@PersonalComments", personalComments);
                if (usePersonalDescription) commandDatabase.Parameters.AddWithValue("@PersonalDescription", personalDescription);
                if (useLocationName) commandDatabase.Parameters.AddWithValue("@LocationName", locationName);
                if (useLocationCity) commandDatabase.Parameters.AddWithValue("@LocationCity", locationCity);
                if (useLocationState) commandDatabase.Parameters.AddWithValue("@LocationState", locationState);
                if (useLocationCountry) commandDatabase.Parameters.AddWithValue("@LocationCountry", locationCountry);
                #endregion

                #region Region names
                if (useRegionNameList && regionNameList != null && regionNameList.Count > 0)
                {
                    for (int index = 0; index < regionNameList.Count; index++) commandDatabase.Parameters.AddWithValue("@MediaPersonalRegionsName" + index.ToString(), regionNameList[index]);
                }
                #endregion

                #region Keywords
                if (useKeywordList && keywords != null && keywords.Count > 0)
                {
                    for (int index = 0; index < keywords.Count; index++) commandDatabase.Parameters.AddWithValue("@MediaPersonalKeywordsKeyword" + index.ToString(), keywords[index]);
                }
                #endregion

                #region Read list of file fullpaths
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listing.Add(Path.Combine(
                            dbTools.ConvertFromDBValString(reader["FileDirectory"]),
                            dbTools.ConvertFromDBValString(reader["FileName"])
                            ));
                    }
                }
                #endregion 
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalAlbums()
        public List<string> ListAllPersonalAlbums(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalAlbum FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalAlbum"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllMediaDateTakenYearAndMonth()
        //SELECT strftime('%Y-%m-%d %H:%M:%S',MediaDateTaken/10000000 - 62135596800,'unixepoch') as 'Date1' FROM MediaMetadata ORDER BY Date1
        //SELECT DISTINCT strftime('%Y-%m',MediaDateTaken/10000000 - 62135596800,'unixepoch') as 'MediaDateTaken' FROM MediaMetadata ORDER BY MediaDateTaken
        public List<string> ListAllMediaDateTakenYearAndMonth(MetadataBrokerType metadataBrokerType)
        {

            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT strftime('%Y-%m',MediaDateTaken/10000000 - 62135596800,'unixepoch') as 'MediaDateTaken' FROM MediaMetadata ORDER BY MediaDateTaken WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listing.Add(dbTools.ConvertFromDBValString(reader["MediaDateTaken"]));
                    }
                }
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalDescriptions()
        public List<string> ListAllPersonalDescriptions(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalDescription FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalDescription"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalTitles()
        public List<string> ListAllPersonalTitles(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalTitle FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalTitle"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalComments()
        public List<string> ListAllPersonalComments(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalComments FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalComments"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllPersonalAuthors()
        public List<string> ListAllPersonalAuthors(MetadataBrokerType metadataBrokerType)
        {

            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT PersonalAuthor FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["PersonalAuthor"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllLocationNames()
        public List<string> ListAllLocationNames(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT LocationName FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["LocationName"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllLocationCities()
        public List<string> ListAllLocationCities(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT LocationCity FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["LocationCity"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllLocationStates()
        public List<string> ListAllLocationStates(MetadataBrokerType metadataBrokerType)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT LocationState FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["LocationState"]));
                }
            }
            return listing;
        }
        #endregion

        #region ListAllLocationCountries()
        public List<string> ListAllLocationCountries(MetadataBrokerType metadataBrokerType)
        {

            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT LocationCountry FROM MediaMetadata WHERE Broker = @Broker";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["LocationCountry"]));
                }
            }
            return listing;
        }
        #endregion


        #region ListAllPersonalRegionNameCountCache
        private Dictionary<MetadataBrokerType, Dictionary<StringNullable, int>> metadataRegionNameCountCache = null;

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameCountCacheClear
        public void ListAllPersonalRegionNameCountCacheClear()
        {
            metadataRegionNameCountCache = null;
            metadataRegionNameCountCache = new Dictionary<MetadataBrokerType, Dictionary<StringNullable, int>>();
        }
        #endregion

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameCount
        private Dictionary<StringNullable, int> ListAllPersonalRegionNameCount(MetadataBrokerType metadataBrokerType)
        {
            //Private due to fake null hack
            Dictionary<StringNullable, int> metadataRegionNameCountDictionary = new Dictionary<StringNullable, int>();

            string sqlCommand =
                "SELECT Name, Count(1) AS CountNames FROM MediaPersonalRegions WHERE Broker = @Broker GROUP BY Name";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = dbTools.ConvertFromDBValString(reader["Name"]);
                        metadataRegionNameCountDictionary.Add(new StringNullable(name), (int)dbTools.ConvertFromDBValInt(reader["CountNames"]));
                    }
                }
            }
            return metadataRegionNameCountDictionary;
        }
        #endregion

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameCountCache
        private Dictionary<StringNullable, int> ListAllPersonalRegionNameCountCache(MetadataBrokerType metadataBrokerType)
        {
            if (metadataRegionNameCountCache == null) metadataRegionNameCountCache = new Dictionary<MetadataBrokerType, Dictionary<StringNullable, int>>();
            if (!metadataRegionNameCountCache.ContainsKey(metadataBrokerType)) metadataRegionNameCountCache.Add(metadataBrokerType, new Dictionary<StringNullable, int>());
            metadataRegionNameCountCache[metadataBrokerType] = ListAllPersonalRegionNameCount(metadataBrokerType);
            return metadataRegionNameCountCache[metadataBrokerType];
        }
        #endregion

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameCountCache
        public List<string> ListAllPersonalRegionNameNotInListCache(MetadataBrokerType metadataBrokerType, List<string> namesdontIncludeList1, List<string> namesdontIncludeList2, int topCount, bool includeEmpty = false)
        {
            List<string> list = new List<string>();
            Dictionary<StringNullable, int> metadataRegionNameCountDictionary = ListAllPersonalRegionNameCountCache(metadataBrokerType);

            var ordered = metadataRegionNameCountDictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            int count = 0;
            if (count < topCount)
            {
                foreach (KeyValuePair<StringNullable, int> keyValuePair in ordered)
                {
                    bool doNotInclude = false;
                    StringNullable brokerRegionName = keyValuePair.Key;
                    if (namesdontIncludeList1 != null && namesdontIncludeList1.Contains(keyValuePair.Key.StringValue)) doNotInclude = true;
                    if (namesdontIncludeList2 != null && namesdontIncludeList2.Contains(keyValuePair.Key.StringValue)) doNotInclude = true;
                    if (!includeEmpty && string.IsNullOrEmpty(brokerRegionName.StringValue)) doNotInclude = true;
                    
                    if (!doNotInclude)
                    {
                        list.Add(keyValuePair.Key.StringValue);
                        count++;
                        if (count >= topCount) break;
                    }
                }
            }
            list.Sort();
            return list;
        }
        #endregion

        #region ListAllPersonalRegionNameCountCache - ListAllPersonalRegionNameTopCountCache
        public List<string> ListAllPersonalRegionNameTopCountCache(MetadataBrokerType metadataBrokerType, int topCount)
        {
            return ListAllPersonalRegionNameNotInListCache(metadataBrokerType, null, null, topCount);
        }
        #endregion

        #region ListAllPersonalRegionsCache
        public List<string> ListAllPersonalRegionsCache(MetadataBrokerType metadataBrokerType)
        {
            return ListAllPersonalRegionNameTopCountCache(metadataBrokerType, int.MaxValue); 
        }
        #endregion

        #endregion

        #region MetadataRegionNamesCache
        Dictionary<MetadataRegionNameKey, List<string>> metadataRegionNamesCache = new Dictionary<MetadataRegionNameKey, List<string>>();

        #region MetadataRegionNamesCache - MetadataRegionNamesCacheClear
        public void MetadataRegionNamesCacheClear()
        {
            metadataRegionNamesCache = null;
            metadataRegionNamesCache = new Dictionary<MetadataRegionNameKey, List<string>>();
        }
        #endregion

        #region MetadataRegionNamesCache - ListAllRegionNamesCache
        public List<string> ListAllRegionNamesCache(MetadataBrokerType metadataBrokerType, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            MetadataRegionNameKey metadataRegionNameKey = new MetadataRegionNameKey(metadataBrokerType, dateTimeFrom, dateTimeTo);
            
            if (MetadataRegionNamesCacheContainsKey(metadataRegionNameKey)) return MetadataRegionNamesCacheGet(metadataRegionNameKey);

            List<string> regionNames = ListAllRegionNames(metadataBrokerType, dateTimeFrom, dateTimeTo);
            if (regionNames != null)
            {
                MetadataRegionNamesCacheUpdate(metadataRegionNameKey, regionNames);
                return regionNames;
            }
            else return null;            
        }
        #endregion

        #region MetadataRegionNamesCache - ListAllRegionNames
        public List<string> ListAllRegionNames(MetadataBrokerType metadataBrokerType, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            List<string> listing = new List<string>();

            string sqlCommand =
                "SELECT DISTINCT Name FROM MediaPersonalRegions " +
                "WHERE Broker = @Broker AND FileDateModified >= @FileDateModifiedFrom AND FileDateModified <= @FileDateModifiedTo";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Broker", (int)metadataBrokerType);
                commandDatabase.Parameters.AddWithValue("@FileDateModifiedFrom", dbTools.ConvertFromDateTimeToDBVal(dateTimeFrom));
                commandDatabase.Parameters.AddWithValue("@FileDateModifiedTo", dbTools.ConvertFromDateTimeToDBVal(dateTimeTo));

                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read()) listing.Add(dbTools.ConvertFromDBValString(reader["Name"]));
                }
            }

            return listing;
        }
        #endregion

        #region MetadataRegionNamesCache - MetadataRegionNamesCacheContainsKey
        private bool MetadataRegionNamesCacheContainsKey(MetadataRegionNameKey key)
        {
            return metadataRegionNamesCache.ContainsKey(key);
        }
        #endregion

        #region MetadataRegionNamesCache - MetadataRegionNamesCacheGet
        private List<string> MetadataRegionNamesCacheGet(MetadataRegionNameKey key)
        {
            return metadataRegionNamesCache[key];
        }
        #endregion

        #region MetadataRegionNamesCache - MetadataRegionNamesCacheUpdate
        private void MetadataRegionNamesCacheUpdate(MetadataRegionNameKey key, List<string> regionNames)
        {
            //Update cache
            if (MetadataRegionNamesCacheContainsKey(key)) metadataRegionNamesCache[key] = regionNames;
            else metadataRegionNamesCache.Add(key, regionNames);
        }
        #endregion

        #endregion

        #region Cache Metadata
        Dictionary<FileEntryBroker, Metadata> metadataCache = new Dictionary<FileEntryBroker, Metadata>();

        #region Cache Metadata - Contains Key
        private bool MetadataCacheContainsKey(FileEntryBroker fileEntryBroker)
        {
            if (!(fileEntryBroker is FileEntryBroker)) fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result
            return metadataCache.ContainsKey(fileEntryBroker);
        }
        #endregion 

        #region Cache Metadata - Get
        private Metadata MetadataCacheGet(FileEntryBroker fileEntryBroker)
        {
            if (!(fileEntryBroker is FileEntryBroker)) fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result 
            return metadataCache[fileEntryBroker];
        }
        #endregion 

        #region Cache Metadata - Read 
        public Metadata ReadMetadataFromCacheOrDatabase(FileEntryBroker fileEntryBroker)
        {
            if (!(fileEntryBroker is FileEntryBroker)) fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result
            if (MetadataCacheContainsKey(fileEntryBroker)) return MetadataCacheGet(fileEntryBroker);

            Metadata metadata = Read(fileEntryBroker);
            MetadataCacheUpdate(fileEntryBroker, metadata);
            return metadata;
        }
        #endregion 

        #region Cache Metadata - Read - CacheOnly
        public Metadata ReadMetadataFromCacheOnly(FileEntryBroker file)
        {
            if (MetadataCacheContainsKey(file))
            {
                return MetadataCacheGet(file);
            }
            return null;
        }
        #endregion 

        #region Cache Metadata - Remove
        private void MetadataCacheRemove(FileEntryBroker fileEntryBroker)
        {
            if (fileEntryBroker == null) return;
            if (!(fileEntryBroker is FileEntryBroker)) fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result

            if (MetadataCacheContainsKey(fileEntryBroker)) metadataCache.Remove(fileEntryBroker);
        }
        #endregion 

        #region Cache Metadata - Updated
        private void MetadataCacheUpdate(FileEntryBroker fileEntryBroker, Metadata metadata)
        {
            if (!(fileEntryBroker is FileEntryBroker)) fileEntryBroker = new FileEntryBroker(fileEntryBroker); //When NOT FileEntryBroker it Will give wrong hash value, and not fint the correct result

            if (metadataCache.ContainsKey(fileEntryBroker)) metadataCache[fileEntryBroker] = metadata;
            else metadataCache.Add(fileEntryBroker, metadata);            
        }
        #endregion 

        #region Cache Metadata - Clear
        public void MetadataCacheClear()
        {
            metadataCache = null;
            metadataCache = new Dictionary<FileEntryBroker, Metadata>();
        }
        #endregion

        #endregion

        #region Cache - Remove
        public void CacheRemove(FileEntryBroker fileEntryBroker)
        {
            MetadataCacheRemove(fileEntryBroker);
            MetadataRegionNamesCacheClear();
            ListAllPersonalRegionNameCountCacheClear();
            ListFileEntryAttributesCacheRemove(new FileBroker(fileEntryBroker.Broker, fileEntryBroker.FileFullPath));
        }
        #endregion 

        #region Cache - Clear
        public void ClearCache()
        {
            MetadataCacheClear();
            MetadataRegionNamesCacheClear();
            ListFileEntryAttributesCacheClear();
            ListAllPersonalRegionNameCountCacheClear();
        }
        #endregion 

        

    }
}
