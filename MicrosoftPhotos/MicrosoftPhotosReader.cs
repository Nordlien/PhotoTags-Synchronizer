#define MonoSqlite
#if MonoSqlite
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

using System;
using System.IO;
using MetadataLibrary;
using SqliteDatabase;
using System.Diagnostics;
using NLog;

namespace MicrosoftPhotos
{
    public class MicrosoftPhotosReader : ImetadataReader
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private SqliteDatabaseUtilities dbTools;
        public MicrosoftPhotosReader()
        {
            dbTools = new SqliteDatabaseUtilities(DatabaseType.SqliteMicrosoftPhotos, 1000, 100);
        }

        public Metadata Read(MetadataBrokerTypes broker, string fullFilePath)
        {        
            Metadata metadata = null; 
            string fileDirectory = Path.GetDirectoryName(fullFilePath);
            string fileName = Path.GetFileName(fullFilePath);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            String query = "SELECT(WITH RECURSIVE " +
                "under_alice(folderid, folderlevel, foldername) AS( " +
                "VALUES(Item_ParentFolderId, 0, NULL) " +
                "UNION ALL " +
                "SELECT Folder_ParentFolderId, under_alice.folderlevel + 1 AS folderlevel, Folder_DisplayName " +
                "FROM Folder JOIN under_alice ON Folder.Folder_Id = under_alice.folderid " +
                "), path_from_root AS( " +
                "SELECT folderlevel, foldername, folderid " +
                "FROM under_alice " +
                "ORDER BY folderlevel DESC " +
                ") " +
                "SELECT  group_concat(foldername, '/') " +
                "FROM path_from_root " +
                "ORDER BY folderlevel DESC) " +
                "AS ItemPath, " +
                "Item_Filename, Item_ParentFolderId, " +
                "Item_Filename, Item_FileSize, " +
                "Location.Location_Name, LocationCountry.LocationCountry_Name,  " +
                "LocationDistrict.LocationDistrict_Name, LocationRegion.LocationRegion_Name,  " +
                "Item_DateTaken, Item_DateCreated, Item_DateModified, Item_Caption, Album.Album_Name, Item_SimpleRating,  " +
                "Item_Width, Item_Height,  " +
                "CameraManufacturer.CameraManufacturer_Text, CameraModel.CameraModel_Text,  " +
                "Item_Latitude, Item_Longitude FROM Item " +
                "INNER JOIN Folder ON Folder.Folder_Id = Item.Item_ParentFolderId " +
                "LEFT OUTER JOIN Location ON Item.Item_LocationId = Location.Location_Id " +
                "LEFT OUTER JOIN LocationCountry ON Location.Location_LocationCountryId = LocationCountry.LocationCountry_Id " +
                "LEFT OUTER JOIN LocationDistrict ON Location.Location_LocationDistrictId = LocationDistrict.LocationDistrict_Id " +
                "LEFT OUTER JOIN LocationRegion ON Location.Location_LocationRegionId = LocationRegion.LocationRegion_Id " +
                "LEFT OUTER JOIN CameraManufacturer ON Item.Item_CameraManufacturerId = CameraManufacturer.CameraManufacturer_Id " +
                "LEFT OUTER JOIN CameraModel ON Item.Item_CameraModelId = CameraModel.CameraModel_Id " +
                "LEFT OUTER JOIN AlbumItemLink ON AlbumItemLink.AlbumItemLink_ItemId = Item.Item_Id " +
                "LEFT OUTER JOIN Album ON Album.Album_Id = AlbumItemLink.AlbumItemLink_AlbumId " +
                "WHERE Item_Filename LIKE @FileName";
                //"WHERE Item_Filename = '" + fullFilePath + "'";
                //"AND ItemPath = '" + path + "%'";



            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(query, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileName", fileName);

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        String itemPath = dbTools.ConvertFromDBValString(reader["ItemPath"]);
                        itemPath = itemPath.Replace("/", "\\");
                        if (fileDirectory.EndsWith(itemPath, StringComparison.InvariantCulture) == true)
                        {
                            //File
                            metadata = new Metadata(broker);
                            metadata.FileName = dbTools.ConvertFromDBValString(reader["Item_Filename"]);
                            //metadata.FileDirectory = dbTools.ConvertFromDBValString(reader["ItemPath"]);
                            metadata.FileDirectory = fileDirectory; //Override path from database, it's not a complete folder path, missing root path
                            metadata.FileSize = dbTools.ConvertFromDBValLong(reader["Item_FileSize"]);
                            metadata.FileDateCreated = dbTools.ConvertSecoundsSince1600ToDateTime(reader["Item_DateCreated"], DateTimeKind.Utc);
                            metadata.FileDateModified = dbTools.ConvertSecoundsSince1600ToDateTime(reader["Item_DateModified"], DateTimeKind.Utc);

                            if (File.Exists(fullFilePath))
                            {
                                //Due to sometimes NULL in Microsoft Database, I always use current file attributes.
                                metadata.FileDateCreated = File.GetCreationTime(fullFilePath);
                                metadata.FileDateModified = File.GetLastWriteTime(fullFilePath);
                                metadata.FileLastAccessed = File.GetLastAccessTime(fullFilePath);
                            }

                            //Personal
                            metadata.PersonalTitle = dbTools.ConvertFromDBValString(reader["Item_Caption"]);
                            //metaData.PersonalDescription = dbTools.ConvertFromDBValString(reader["Item_Caption"]);
                            metadata.PersonalRating = dbTools.ConvertFromDBValByte(reader["Item_SimpleRating"]);
                            //metaData.PersonalAuthor = dbTools.ConvertFromDBValString(reader["Unknown"]);
                            metadata.PersonalAlbum = dbTools.ConvertFromDBValString(reader["Album_Name"]);

                            //Media
                            metadata.MediaWidth = dbTools.ConvertFromDBValInt(reader["Item_Width"]);
                            metadata.MediaHeight = dbTools.ConvertFromDBValInt(reader["Item_Height"]);
                            metadata.MediaDateTaken = dbTools.ConvertSecoundsSince1600ToDateTime(reader["Item_DateTaken"], DateTimeKind.Local);

                            //Camera
                            metadata.CameraMake = dbTools.ConvertFromDBValString(reader["CameraManufacturer_Text"]);
                            metadata.CameraModel = dbTools.ConvertFromDBValString(reader["CameraModel_Text"]);

                            //Location
                            metadata.LocationName = dbTools.ConvertFromDBValString(reader["Location_Name"]);
                            metadata.LocationCountry = dbTools.ConvertFromDBValString(reader["LocationCountry_Name"]);
                            metadata.LocationCity = dbTools.ConvertFromDBValString(reader["LocationDistrict_Name"]);
                            metadata.LocationState = dbTools.ConvertFromDBValString(reader["LocationRegion_Name"]);

                            metadata.LocationLatitude = dbTools.ConvertFromDBValDouble(reader["Item_Latitude"]);
                            metadata.LocationLongitude = dbTools.ConvertFromDBValDouble(reader["Item_Longitude"]);
                            break;
                        }
                    }
                    
                }
            }
            if (metadata == null) return null;

            stopwatch.Stop();
            Logger.Trace("---Microsoft photos read metadata {0}ms {1}", stopwatch.ElapsedMilliseconds, fullFilePath);
            stopwatch.Restart();

            query = "SELECT(WITH RECURSIVE " +
                "under_alice(folderid, folderlevel, foldername) AS( " +
                "VALUES(Item_ParentFolderId, 0, NULL) " +
                "UNION ALL " +
                "SELECT Folder_ParentFolderId, under_alice.folderlevel + 1 AS folderlevel, Folder_DisplayName " +
                "FROM Folder JOIN under_alice ON Folder.Folder_Id = under_alice.folderid " +
                "), path_from_root AS( " +
                "SELECT folderlevel, foldername, folderid " +
                "FROM under_alice " +
                "ORDER BY folderlevel DESC " +
                ") " +
                "SELECT  group_concat(foldername, '/') " +
                "FROM path_from_root " +
                "ORDER BY folderlevel DESC) " +
                "AS ItemPath, Item_Filename, Item_FileSize, " +
                "Person_Name, Face_Rect_Left, Face_Rect_Top, Face_Rect_Width, Face_Rect_Height " +
                "FROM Item " +
                "INNER JOIN Folder ON Folder.Folder_Id = Item.Item_ParentFolderId " +
                "INNER JOIN Face ON Face.Face_ItemId = Item.Item_Id " +
                "INNER JOIN Person ON Person.Person_Id = Face_PersonId " +
                "WHERE " +
                "Item.Item_Filename LIKE @FileName";
            //"AND ItemPath = '" + path + "%'";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(query, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileName", fileName);

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {                   
                    while (reader.Read())
                    {
                        String itemPath = dbTools.ConvertFromDBValString(reader["ItemPath"]);
                        itemPath = itemPath.Replace("/", "\\");
                        if (fileDirectory.EndsWith(itemPath, StringComparison.InvariantCulture) == true)
                        {
                            RegionStructure region = new RegionStructure();
                            region.Name = dbTools.ConvertFromDBValString(reader["Person_Name"]);
                            region.AreaX = (float)dbTools.ConvertFromDBValDouble(reader["Face_Rect_Left"]);
                            region.AreaY = (float)dbTools.ConvertFromDBValDouble(reader["Face_Rect_Top"]);
                            region.AreaWidth = (float)dbTools.ConvertFromDBValDouble(reader["Face_Rect_Width"]);
                            region.AreaHeight = (float)dbTools.ConvertFromDBValDouble(reader["Face_Rect_Height"]);
                            region.Type = "Face";
                            region.RegionStructureType = RegionStructureTypes.MicrosoftPhotosDatabase;
                            metadata.PersonalRegionListAddIfNotExists(region);
                        }


                    }
                }
            }

            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds>200) Logger.Trace("---Microsoft photos read rect {0}ms {1}", stopwatch.ElapsedMilliseconds, fullFilePath);
            
            stopwatch.Restart();

            query = "SELECT (WITH RECURSIVE under_alice(folderid, folderlevel, foldername) AS(VALUES(Item_ParentFolderId, 0, NULL) " + 
                "UNION ALL SELECT Folder_ParentFolderId, under_alice.folderlevel + 1 AS folderlevel, Folder_DisplayName " +
                "FROM Folder JOIN under_alice " +
                "ON Folder.Folder_Id = under_alice.folderid), path_from_root AS " +
                "(SELECT folderlevel, foldername, folderid FROM under_alice ORDER BY folderlevel DESC) " +
                "SELECT group_concat(foldername, '/') FROM path_from_root ORDER BY folderlevel DESC) AS ItemPath, " +
                "Item_Filename, Item_FileSize, Item.Item_DateModified, Item.Item_DateCreated " +
                ", Item.Item_Id " +
				", ItemTags1.ItemTags_TagId " +
                //Workaround, I have now clue why, when ItemTags_Confidence not selected this way, then indexes wasn't used
                ", (SELECT ItemTags_Confidence FROM ItemTags AS ItemTags2 Where ItemTags2.ItemTags_TagId = ItemTags1.ItemTags_TagId ) AS ItemTags_Confidence " +               
                ", (SELECT TagVariant_Text FROM TagVariant Where TagVariant.TagVariant_TagResourceId = Tag.Tag_ResourceId ) AS TagVariant_Text " +
                "FROM Item " +
                "INNER JOIN Folder ON Folder.Folder_Id = Item.Item_ParentFolderId " +
                "INNER JOIN ItemTags AS ItemTags1 ON ItemTags1.ItemTags_ItemId = Item.Item_Id " +
                "INNER JOIN Tag ON ItemTags1.ItemTags_TagId = Tag.Tag_Id " +
                "WHERE Item.Item_Filename LIKE @FileName";
    
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(query, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileName", fileName);

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                        String itemPath = dbTools.ConvertFromDBValString(reader["ItemPath"]);
                        itemPath = itemPath.Replace("/", "\\");
                        if (fileDirectory.EndsWith(itemPath, StringComparison.InvariantCulture) == true)
                        {
                            KeywordTag keywordTag = new KeywordTag(
                                dbTools.ConvertFromDBValString(reader["TagVariant_Text"]), 
                                (double)dbTools.ConvertFromDBValDouble(reader["ItemTags_Confidence"])
                                );

                            if (metadata.PersonalKeywordTags.Contains(keywordTag))
                            { 
                            //DEBUG
                            }
                            metadata.PersonalKeywordTagsAddIfNotExists(keywordTag);

                            
                        }
                    }

                }
            }

            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 200) Logger.Trace("---Microsoft photos read variant {0}ms {1}", stopwatch.ElapsedMilliseconds, fullFilePath);
            

            return metadata;

        }

    }
}
