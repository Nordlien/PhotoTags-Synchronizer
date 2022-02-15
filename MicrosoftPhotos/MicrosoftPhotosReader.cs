#define MicrosoftDataSqlite

#if MonoSqlite
using Mono.Data.Sqlite;
#elif MicrosoftDataSqlite
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

using System;
using System.IO;
using MetadataLibrary;
using SqliteDatabase;
using NLog;

namespace MicrosoftPhotos
{
    public class MicrosoftPhotosReader : ImetadataReader
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private SqliteDatabaseUtilities dbToolsMicrosoftReader;
        public MicrosoftPhotosReader()
        {
            dbToolsMicrosoftReader = new SqliteDatabaseUtilities(DatabaseType.SqliteMicrosoftPhotos, 1000, 100);
        }

        public Metadata Read(MetadataBrokerType broker, string fullFilePath)
        {        
            Metadata metadata = null; 
            string fileDirectory = Path.GetDirectoryName(fullFilePath);
            string fileName = Path.GetFileName(fullFilePath);

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



            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(query, dbToolsMicrosoftReader.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileName", fileName);

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        String itemPath = dbToolsMicrosoftReader.ConvertFromDBValString(reader["ItemPath"]);
                        itemPath = itemPath.Replace("/", "\\");
                        if (fileDirectory.EndsWith(itemPath, StringComparison.InvariantCulture) == true)
                        {
                            //File
                            metadata = new Metadata(broker);
                            metadata.FileName = dbToolsMicrosoftReader.ConvertFromDBValString(reader["Item_Filename"]);
                            //metadata.FileDirectory = dbTools.ConvertFromDBValString(reader["ItemPath"]);
                            metadata.FileDirectory = fileDirectory; //Override path from database, it's not a complete folder path, missing root path
                            metadata.FileSize = dbToolsMicrosoftReader.ConvertFromDBValLong(reader["Item_FileSize"]);
                            metadata.FileDateCreated = dbToolsMicrosoftReader.ConvertSecoundsSince1600ToDateTime(reader["Item_DateCreated"], DateTimeKind.Utc);
                            metadata.FileDateModified = dbToolsMicrosoftReader.ConvertSecoundsSince1600ToDateTime(reader["Item_DateModified"], DateTimeKind.Utc);

                            if (metadata.FileDateCreated == null ||
                                metadata.FileDateModified == null ||
                                metadata.FileDateAccessed == null ||
                                File.Exists(fullFilePath))
                            {
                                try
                                {
                                    //Due to sometimes NULL in Microsoft Database, I always use current file attributes.
                                    FileInfo fileInfo = new FileInfo(fullFilePath);
                                    metadata.FileDateCreated = fileInfo.CreationTime; //File.GetCreationTime(fullFilePath);
                                    metadata.FileDateModified = fileInfo.LastWriteTime; //File.GetLastWriteTime(fullFilePath);
                                    metadata.FileDateAccessed = fileInfo.LastAccessTime; //File.GetLastAccessTime(fullFilePath);
                                } catch (Exception ex)
                                {
                                    Logger.Error(ex);
                                    metadata.FileDateCreated = DateTime.Now;
                                    metadata.FileDateModified = metadata.FileDateCreated; //File.GetLastWriteTime(fullFilePath);
                                    metadata.FileDateAccessed = metadata.FileDateCreated; //File.GetLastAccessTime(fullFilePath);
                                }
                            }

                            //Personal
                            metadata.PersonalTitle = dbToolsMicrosoftReader.ConvertFromDBValString(reader["Item_Caption"]);
                            //metaData.PersonalDescription = dbTools.ConvertFromDBValString(reader["Item_Caption"]);
                            metadata.PersonalRating = dbToolsMicrosoftReader.ConvertFromDBValByte(reader["Item_SimpleRating"]);
                            //metaData.PersonalAuthor = dbTools.ConvertFromDBValString(reader["Unknown"]);
                            metadata.PersonalAlbum = dbToolsMicrosoftReader.ConvertFromDBValString(reader["Album_Name"]);

                            //Media
                            metadata.MediaWidth = dbToolsMicrosoftReader.ConvertFromDBValInt(reader["Item_Width"]);
                            metadata.MediaHeight = dbToolsMicrosoftReader.ConvertFromDBValInt(reader["Item_Height"]);
                            metadata.MediaDateTaken = dbToolsMicrosoftReader.ConvertSecoundsSince1600ToDateTime(reader["Item_DateTaken"], DateTimeKind.Local);

                            //Camera
                            metadata.CameraMake = dbToolsMicrosoftReader.ConvertFromDBValString(reader["CameraManufacturer_Text"]);
                            metadata.CameraModel = dbToolsMicrosoftReader.ConvertFromDBValString(reader["CameraModel_Text"]);

                            //Location
                            metadata.LocationName = dbToolsMicrosoftReader.ConvertFromDBValString(reader["Location_Name"]);
                            metadata.LocationCountry = dbToolsMicrosoftReader.ConvertFromDBValString(reader["LocationCountry_Name"]);
                            metadata.LocationCity = dbToolsMicrosoftReader.ConvertFromDBValString(reader["LocationDistrict_Name"]);
                            metadata.LocationState = dbToolsMicrosoftReader.ConvertFromDBValString(reader["LocationRegion_Name"]);

                            metadata.LocationLatitude = dbToolsMicrosoftReader.ConvertFromDBValFloat(reader["Item_Latitude"]);
                            metadata.LocationLongitude = dbToolsMicrosoftReader.ConvertFromDBValFloat(reader["Item_Longitude"]);

                            if (metadata.LocationLatitude == 0 && metadata.LocationLongitude == 0) //Due to bug in Microsoft Photos Gallery
                            {
                                metadata.LocationLatitude = null;
                                metadata.LocationLongitude = null;
                            }
                            break;
                        }
                    }
                    
                }
            }
            if (metadata == null) return null;

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

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(query, dbToolsMicrosoftReader.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileName", fileName);

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {                   
                    while (reader.Read())
                    {
                        String itemPath = dbToolsMicrosoftReader.ConvertFromDBValString(reader["ItemPath"]);
                        itemPath = itemPath.Replace("/", "\\");
                        if (fileDirectory.EndsWith(itemPath, StringComparison.InvariantCulture) == true)
                        {
                            RegionStructure region = new RegionStructure();
                            region.Name = dbToolsMicrosoftReader.ConvertFromDBValString(reader["Person_Name"]);
                            region.AreaX = (float)dbToolsMicrosoftReader.ConvertFromDBValFloat(reader["Face_Rect_Left"]);
                            region.AreaY = (float)dbToolsMicrosoftReader.ConvertFromDBValFloat(reader["Face_Rect_Top"]);
                            region.AreaWidth = (float)dbToolsMicrosoftReader.ConvertFromDBValFloat(reader["Face_Rect_Width"]);
                            region.AreaHeight = (float)dbToolsMicrosoftReader.ConvertFromDBValFloat(reader["Face_Rect_Height"]);
                            region.Type = "Face";
                            region.RegionStructureType = RegionStructureTypes.MicrosoftPhotosDatabase;
                            metadata.PersonalRegionListAddIfNotExists(region);
                        }


                    }
                }
            }

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
    
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(query, dbToolsMicrosoftReader.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@FileName", fileName);

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                        String itemPath = dbToolsMicrosoftReader.ConvertFromDBValString(reader["ItemPath"]);
                        itemPath = itemPath.Replace("/", "\\");
                        if (fileDirectory.EndsWith(itemPath, StringComparison.InvariantCulture) == true)
                        {
                            KeywordTag keywordTag = new KeywordTag(
                                dbToolsMicrosoftReader.ConvertFromDBValString(reader["TagVariant_Text"]), 
                                (float)dbToolsMicrosoftReader.ConvertFromDBValFloat(reader["ItemTags_Confidence"])
                                );
                            metadata.PersonalKeywordTagsAddIfNotExists(keywordTag);
                        }
                    }

                }
            }
            return metadata;

        }

    }
}
