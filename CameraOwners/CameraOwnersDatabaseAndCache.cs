using SqliteDatabase;
using System.Collections.Generic;


namespace CameraOwners
{

    public class CameraOwnersDatabaseCache    
    {
        public static readonly string MissingLocationsOwners = "(Need to import GPS location)\t";
        
        private readonly SqliteDatabaseUtilities dbTools;
        public CameraOwnersDatabaseCache(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }

        private static List<CameraOwner> cameraMakeModelAndOwnersCache = null;

        public void CameraMakeModelAndOwnerMakeDirty()
        {
            cameraMakeModelAndOwnersCache = null;
        }

        public bool IsCameraMakeModelAndOwnerDirty()
        {
            return cameraMakeModelAndOwnersCache != null;
        }

        #region ReadCameraMakeModelAndOwners
        public List<CameraOwner> ReadCameraMakeModelAndOwners()
        {
            if (cameraMakeModelAndOwnersCache == null)
            {
                cameraMakeModelAndOwnersCache = new List<CameraOwner>();
                string sqlCommand = "SELECT CameraMake, CameraModel, UserAccount FROM CameraOwner";
                
                using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
                {
                    //commandDatabase.Prepare();
                    using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            CameraOwner cameraOwner = new CameraOwner(
                                dbTools.ConvertFromDBValString(reader["CameraMake"]),
                                dbTools.ConvertFromDBValString(reader["CameraModel"]),
                                dbTools.ConvertFromDBValString(reader["UserAccount"]));
                            cameraMakeModelAndOwnersCache.Add(cameraOwner);
                        }
                    }
                }
            }
            return cameraMakeModelAndOwnersCache;
        }
        #endregion

        #region ReadCameraMakeModelAndOwnersThatNotExist from MediaMetadata
        public List<CameraOwner> ReadCameraMakeModelAndOwnersThatNotExist(List<CameraOwner> cameraOwners)
        {
            string sqlCommand = "SELECT DISTINCT CameraMake, CameraModel, NULL as UserAccount FROM MediaMetadata";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        CameraOwner cameraOwner = new CameraOwner(
                            dbTools.ConvertFromDBValString(reader["CameraMake"]),
                            dbTools.ConvertFromDBValString(reader["CameraModel"]),
                            dbTools.ConvertFromDBValString(reader["UserAccount"]));                        
                        if (!string.IsNullOrWhiteSpace(cameraOwner.Make) &&
                            !string.IsNullOrWhiteSpace(cameraOwner.Model) &&
                            !CameraOwner.CameraMakeModelExistInList(cameraOwners, cameraOwner)) cameraOwners.Add(cameraOwner);
                    }
                }
            }
            return cameraOwners;
        }
        #endregion

        #region SaveCameraMakeModelAndOwner
        public void SaveCameraMakeModelAndOwner(CommonDatabaseTransaction commonDatabaseTransaction, CameraOwner cameraOwner)
        {
            if (cameraOwner == null) return;
            if (string.IsNullOrWhiteSpace(cameraOwner.Make)) cameraOwner.Make = CameraOwner.UnknownMake;
            if (string.IsNullOrWhiteSpace(cameraOwner.Model)) cameraOwner.Model = CameraOwner.UnknownModel;
            //if (string.IsNullOrWhiteSpace(cameraOwner.Owner)) cameraOwner.Owner = CameraOwner.UnknownOwner;

            string sqlCommand =
                "DELETE FROM CameraOwner WHERE " +
                "CameraMake = @CameraMake AND CameraModel = @CameraModel";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, commonDatabaseTransaction.DatabaseTransaction))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@CameraMake", cameraOwner.Make);
                commandDatabase.Parameters.AddWithValue("@CameraModel", cameraOwner.Model);
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }

            sqlCommand =
                    "INSERT INTO CameraOwner (CameraMake, CameraModel, UserAccount) " +
                    "Values (@CameraMake, @CameraModel, @UserAccount)";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, commonDatabaseTransaction.DatabaseTransaction))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@CameraMake", cameraOwner.Make);
                commandDatabase.Parameters.AddWithValue("@CameraModel", cameraOwner.Model);
                commandDatabase.Parameters.AddWithValue("@UserAccount", cameraOwner.Owner);
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }

            MakeCameraOwnersDirty();
        }
        #endregion 

        #region Camera Owner Cache - MakeCameraOwnersDirty()
        private static List<string> cameraOwnerCache = null;
        public void MakeCameraOwnersDirty()
        {
            cameraOwnerCache = null;
        }
        #endregion 

        #region ReadCameraOwners
        public List<string> ReadCameraOwners()
        {
            if (cameraOwnerCache == null)
            {
                string sqlCommand = "SELECT DISTINCT UserAccount FROM LocationSource";
                using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
                {
                    //commandDatabase.Prepare();
                    commandDatabase.ExecuteNonQuery();      // Execute the query

                    using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                    {
                        cameraOwnerCache = new List<string>();
                        while (reader.Read())
                        {
                            cameraOwnerCache.Add(dbTools.ConvertFromDBValString(reader["UserAccount"]));
                        }
                    }
                    if (cameraOwnerCache.Count == 0) cameraOwnerCache.Add(MissingLocationsOwners);
                }
            }
            return cameraOwnerCache;

        }
        #endregion 

        #region GetOwenerForCameraMakeModel
        public string GetOwenerForCameraMakeModel (string make, string model)
        {
            if (string.IsNullOrWhiteSpace(make)) make = CameraOwner.UnknownMake;
            if (string.IsNullOrWhiteSpace(model)) model = CameraOwner.UnknownModel;

            if (cameraMakeModelAndOwnersCache == null) ReadCameraMakeModelAndOwners();
            foreach (CameraOwner cameraOwner in cameraMakeModelAndOwnersCache)
            {
                if (cameraOwner.Make == make && cameraOwner.Model == model) 
                {
                    return cameraOwner.Owner;
                }
            }
            return null;
        }
        #endregion

    }
}
