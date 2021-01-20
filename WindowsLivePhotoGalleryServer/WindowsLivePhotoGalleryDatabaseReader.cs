using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management;
using System.Text;
using ChristianHelle.DatabaseTools.SqlCe;
using MetadataLibrary;

namespace WindowsLivePhotoGalleryServer
{
    public class WindowsLivePhotoGalleryDatabaseReader : ImetadataReader
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private string dataSource = "";
        private ISqlCeDatabase database;
        

        public void Connect(string path)
        {
            dataSource = path;
            if (!File.Exists(dataSource))
                throw new InvalidOperationException("Unable to find Windows Live Photo Gallery database file:" + dataSource);

            var connstr = string.Format("Data Source={0}; Password={1};", path, "");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            database = SqlCeDatabaseFactory.Create(connstr);
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 200) Logger.Info("Connect Compact SQL CE " + stopwatch.ElapsedMilliseconds.ToString());
        }
        
        private string convertSerialToDriveLetter(string hexSerialNumber)
        {
            var driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject d in driveQuery.Get())
            {
                var deviceId = d.Properties["DeviceId"].Value;
                var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
                var partitionQuery = new ManagementObjectSearcher(partitionQueryText);
                foreach (ManagementObject p in partitionQuery.Get())
                {
                    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", p.Path.RelativePath);
                    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
                    foreach (ManagementObject ld in logicalDriveQuery.Get())
                    {
                        string driveId = (String)Convert.ToString(ld.Properties["DeviceId"].Value); // C:
                        string volumeSerial = (String)ld.Properties["VolumeSerialNumber"].Value; // 12345678
                        if (volumeSerial == hexSerialNumber)
                        {
                            return driveId + "\\";
                        }
                    }
                }
            }
            throw (new Exception ("Can't find Windows Live Photo Gallery drive id:" + hexSerialNumber));
        }

        private int convertSerialToDriveToId(string driveLetter)
        {
            var driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject d in driveQuery.Get())
            {
                var deviceId = d.Properties["DeviceId"].Value;
                var partitionQuery = new ManagementObjectSearcher(string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath));

                foreach (ManagementObject p in partitionQuery.Get())
                {
                    var logicalDriveQuery = new ManagementObjectSearcher(string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", p.Path.RelativePath));
                    foreach (ManagementObject ld in logicalDriveQuery.Get())
                    {
                        string driveId = (String)Convert.ToString(ld.Properties["DeviceId"].Value); // C:
                        string volumeSerial = (String)ld.Properties["VolumeSerialNumber"].Value; // 12345678
                        if (driveId == driveLetter)
                        {
                            logicalDriveQuery.Dispose();
                            partitionQuery.Dispose();
                            driveQuery.Dispose();
                            return Convert.ToInt32(volumeSerial, 16); ;
                        }
                    }
                    logicalDriveQuery.Dispose();
                }
                partitionQuery.Dispose();
            }
            driveQuery.Dispose();
            throw (new Exception("Can't find Windows Live Photo Gallery drive letter for serial id: " + driveLetter));
        }

        static private Dictionary<string, int> dictionarySerialToDriveToId = new Dictionary<string, int>();

        public Metadata Read(MetadataBrokerType broker, string fullFilePath)
        {
            int serialNumber;
            if (string.IsNullOrWhiteSpace(fullFilePath)) return null;

            string driverLetter = fullFilePath.Substring(0, 2);
            if (dictionarySerialToDriveToId.ContainsKey(driverLetter))
            {
                serialNumber = dictionarySerialToDriveToId[driverLetter];
            } else
            {
                serialNumber = convertSerialToDriveToId(driverLetter);
                dictionarySerialToDriveToId.Add(driverLetter, serialNumber);
            }
           
            return ReadMetadata(broker, 
                Path.GetFileName(fullFilePath), 
                Path.GetDirectoryName(fullFilePath).Substring(2), 
                Path.GetDirectoryName(fullFilePath), serialNumber);
        }

        private Metadata ReadMetadata(MetadataBrokerType broker, string filename, string windowsLivePhotoGalleryDirectory, string directory, int serialNumber)
        {
            Metadata metadata = null;
            
            var errors = new StringBuilder();
            var messages = new StringBuilder();

            string sql = "SELECT tblVolume.Label, tblPath.Path, tblObject.Filename, tblObject.FileSize, " +
	            "tblLocation.LocationName, tblLocation.LocationLat, tblLocation.LocationLong, tblObject.Latitude, tblObject.Longitude, " +
                "tblObject.DateModified, tblObject.Title, tblObject.\"Desc\" as Desciption, tblObject.DateTaken, tblObject.Rating, tblObject.Author, tblObject.LabelCount, " +
                "tblObject.ResolutionX, tblObject.ResolutionY, tblObject.CameraMake, tblObject.CameraModel, " +
                "tblObject.CameraShutterSpeed, tblObject.CameraFocalLength, tblObject.CameraAperture, tblObject.CameraISO, " +
                "tblObject.VideoBitrate, tblObject.VideoFramerate, tblObject.MediaDuration " +
                "FROM tblObject " +
                "INNER JOIN tblPath ON tblObject.FilePathId = tblPath.PathId " +
                "INNER JOIN tblVolume ON tblPath.VolumeId = tblVolume.VolumeId " +
                "LEFT OUTER JOIN tblocationUsage ON tblocationUsage.ObjectID = tblObject.ObjectID " +
                "LEFT OUTER JOIN tblLocation ON tblocationUsage.LocationId = tblLocation.LocationID " +
                "WHERE " +
                "tblObject.Filename = '" + filename + "' " +
                "AND tblPath.Path = '" + windowsLivePhotoGalleryDirectory + "' " +
                "AND tblVolume.SerialNo = " + serialNumber.ToString(CultureInfo.InvariantCulture);

            

            int resultCount;
            var result = database.ExecuteQuery(sql, errors, messages, out resultCount) as DataSet;

            if (result == null) return metadata; //No point to contine, when header of information missing.

            foreach (DataTable table in result.Tables)
            {
                if (table.Rows.Count > 0)
                {
                    metadata = new Metadata(broker);

                    //File
                    metadata.FileName = table.Rows[0].Field<String>("Filename");
                    //metadata.FileDirectory = table.Rows[0].Field<String>("Path");
                    metadata.FileDirectory = directory; //Override path from database, it's not a complete folder path, missing root path
                    metadata.FileSize = table.Rows[0].Field<Int64>("FileSize");

                    DateTime newDateTime = new DateTime(new DateTime(1600, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc).AddSeconds(1).Ticks + table.Rows[0].Field<long>("DateModified"));
                    metadata.FileDateModified = newDateTime.ToLocalTime();

                    string fullFilePath = Path.Combine(metadata.FileDirectory, metadata.FileName);
                    if (File.Exists(Path.Combine(metadata.FileDirectory, metadata.FileName))) 
                    {
                        metadata.FileDateModified = File.GetLastWriteTime(fullFilePath);
                        metadata.FileDateCreated = File.GetCreationTime(fullFilePath);
                        metadata.FileLastAccessed = File.GetLastAccessTime(fullFilePath);
                    }

                    //Personal
                    metadata.PersonalTitle = table.Rows[0].Field<String>("Title");
                    metadata.PersonalDescription = table.Rows[0].Field<String>("Desciption");
                    metadata.PersonalRating = table.Rows[0].Field<Byte?>("Rating");
                    metadata.PersonalAuthor = table.Rows[0].Field<String>("Author");

                    //Media
                    metadata.MediaWidth = table.Rows[0].Field<Int32?>("ResolutionX");
                    metadata.MediaHeight = table.Rows[0].Field<Int32?>("ResolutionY");
                    metadata.MediaDateTaken = table.Rows[0].Field<DateTime?>("DateTaken");

                    //Camera
                    metadata.CameraMake = table.Rows[0].Field<String>("CameraMake");
                    metadata.CameraModel = table.Rows[0].Field<String>("CameraModel");

                    //Location
                    metadata.LocationName = table.Rows[0].Field<String>("LocationName");
                    metadata.LocationLatitude = (float?)table.Rows[0].Field<double?>("LocationLat");    //Fixed address location, tagged location in map
                    metadata.LocationLongitude = (float?)table.Rows[0].Field<double?>("LocationLong");  //Fixed address location, tagged location in map
                                                                                                //If -999, GPS location missiong, use tagged location
                    if (table.Rows[0].Field<double?>("Latitude") != -999) metadata.LocationLatitude = (float?)table.Rows[0].Field<double?>("Latitude");       //Real GPS location
                    if (table.Rows[0].Field<double?>("Longitude") != -999) metadata.LocationLongitude = (float?)table.Rows[0].Field<double?>("Longitude");     //Real GPS location
                }
            }

            if (metadata == null) return metadata;

            sql = "SELECT tblVolume.Label, tblPath.Path, tblObject.Filename, tblPerson.Name,  tblRegion.\"Top\", tblRegion.\"Left\"," +
                "tblRegion.Width, tblRegion.Height " +
                "FROM tblObject " +
                "INNER JOIN tblPath ON tblObject.FilePathId = tblPath.PathId " +
                "INNER JOIN tblVolume ON tblPath.VolumeId = tblVolume.VolumeId " +
                "INNER JOIN tblRegion ON tblRegion.ObjectID = tblObject.ObjectId " +
                "INNER JOIN tblPerson ON tblRegion.PersonID = tblPerson.PersonId " +
                "WHERE " +
                "tblObject.Filename = '" + filename + "' " +
                "AND tblPath.Path = '" + windowsLivePhotoGalleryDirectory + "' " +
                "AND tblVolume.SerialNo = " + serialNumber.ToString();
            
            result = database.ExecuteQuery(sql, errors, messages, out resultCount) as DataSet;
            
            if (result != null)
            foreach (DataTable table in result.Tables)
            {
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        RegionStructure region = new RegionStructure();

                        region.Name = (String)row["Name"];
                        region.AreaX = (float)Math.Round((double)row["Left"], 5);
                        region.AreaY = (float)Math.Round((double)row["Top"], 5);
                        region.AreaWidth = (float)Math.Round((double)row["Width"], 5);
                        region.AreaHeight = (float)Math.Round((double)row["Height"], 5);
                        region.Type = "Face";
                        region.RegionStructureType = RegionStructureTypes.WindowsLivePhotoGalleryDatabase;
                        metadata.PersonalRegionListAddIfNotExists(region);

                    }
                }
            }

            sql = "SELECT tblVolume.Label, tblPath.Path, tblObject.Filename, LabelName " +
                "FROM tblObject " +
                "INNER JOIN tblPath ON tblObject.FilePathId = tblPath.PathId " +
                "INNER JOIN tblVolume ON tblPath.VolumeId = tblVolume.VolumeId " +
                "INNER JOIN tblLabelUsage ON tblLabelUsage.ObjectID = tblObject.ObjectId " +
                "INNER JOIN tblLabel ON tblLabel.LabelID = tblLabelUsage.LabelId " +
                "WHERE " +
                "tblObject.Filename = '" + filename + "' " +
                "AND tblPath.Path = '" + windowsLivePhotoGalleryDirectory + "' " +
                "AND tblVolume.SerialNo = " + serialNumber.ToString();

            result = database.ExecuteQuery(sql, errors, messages, out resultCount) as DataSet;
            
            if (result != null) foreach (DataTable table in result.Tables)
            {
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag((String)row["LabelName"]));
                    }
                }
            }
            return metadata;
        }

        

    }
}
