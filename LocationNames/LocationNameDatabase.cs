#define MonoSqlite
#if MonoSqlite
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
#endif
using SqliteDatabase;
using MetadataLibrary;

namespace LocationNames
{
    class LocationNameDatabase
    {
        private SqliteDatabaseUtilities dbTools;
        public LocationNameDatabase(SqliteDatabaseUtilities databaseTools)
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

        public void WriteLocationName(Metadata metadata)
        {
            string sqlCommand =
                "INSERT INTO LocationName (Latitude, Longitude, Name, City, Province, Country) " +
                "Values (@Latitude, @Longitude, @Name, @City, @Province, @Country)";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                metadata.LocationName = string.IsNullOrEmpty(metadata.LocationName) ? null : metadata.LocationName;
                metadata.LocationState = string.IsNullOrEmpty(metadata.LocationState) ? null : metadata.LocationState;
                metadata.LocationCity = string.IsNullOrEmpty(metadata.LocationCity) ? null : metadata.LocationCity;
                metadata.LocationCountry = string.IsNullOrEmpty(metadata.LocationCountry) ? null : metadata.LocationCountry;

                commandDatabase.Parameters.AddWithValue("@Latitude", metadata.LocationLatitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", metadata.LocationLongitude);
                commandDatabase.Parameters.AddWithValue("@Name", metadata.LocationName);
                commandDatabase.Parameters.AddWithValue("@City", metadata.LocationCity);
                commandDatabase.Parameters.AddWithValue("@Province", metadata.LocationState);
                commandDatabase.Parameters.AddWithValue("@Country", metadata.LocationCountry);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        public void DeleteLocationName(float latitude, float longitude)
        {
            string sqlCommand = "DELETE FROM LocationName WHERE Latitude = @Latitude AND Longitude = @Longitude";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Latitude", latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", longitude);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }

        public void UpdateLocationName(Metadata metadata)
        {
            string sqlCommand =
                "UPDATE LocationName SET " +
                "Name = @Name, " +
                "City = @City, " +
                "Province = @Province, " +
                "Country = @Country " +
                "WHERE Latitude = @Latitude AND Longitude = @Longitude";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                metadata.LocationName = string.IsNullOrEmpty(metadata.LocationName) ? null : metadata.LocationName;
                metadata.LocationState = string.IsNullOrEmpty(metadata.LocationState) ? null : metadata.LocationState;
                metadata.LocationCity = string.IsNullOrEmpty(metadata.LocationCity) ? null : metadata.LocationCity;
                metadata.LocationCountry = string.IsNullOrEmpty(metadata.LocationCountry) ? null : metadata.LocationCountry;

                commandDatabase.Parameters.AddWithValue("@Latitude", metadata.LocationLatitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", metadata.LocationLongitude);
                commandDatabase.Parameters.AddWithValue("@Name", metadata.LocationName);
                commandDatabase.Parameters.AddWithValue("@City", metadata.LocationCity);
                commandDatabase.Parameters.AddWithValue("@Province", metadata.LocationState);
                commandDatabase.Parameters.AddWithValue("@Country", metadata.LocationCountry);
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }


        public Metadata ReadLocationName(double latitude, double longitude)
        {
            Metadata metadata = null;

            string sqlCommand = "SELECT MAX (ABS(Latitude - @Latitude), ABS(Longitude - @Longitude)) AS Distance, " +
                "Latitude, Longitude, Name, City, Province, Country FROM LocationName WHERE " +
                "Latitude > (@Latitude - 0.001) AND (Latitude< @Latitude + 0.001)" +
                "AND Longitude > (@Longitude - 0.001) AND Longitude< (@Longitude + 0.001) " +
                "ORDER BY Distance DESC";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                commandDatabase.Parameters.AddWithValue("@Latitude", latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", longitude);
                commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        metadata = new Metadata(MetadataBrokerTypes.NominatimAPI);
                        metadata.LocationLatitude = dbTools.ConvertFromDBValFloat(reader["Latitude"]);
                        metadata.LocationLongitude = dbTools.ConvertFromDBValFloat(reader["Longitude"]);
                        metadata.LocationName = dbTools.ConvertFromDBValString(reader["Name"]);
                        metadata.LocationCity = dbTools.ConvertFromDBValString(reader["City"]);
                        metadata.LocationState = dbTools.ConvertFromDBValString(reader["Province"]);
                        metadata.LocationCountry = dbTools.ConvertFromDBValString(reader["Country"]);

                        metadata.LocationName = string.IsNullOrEmpty(metadata.LocationName) ? null : metadata.LocationName;
                        metadata.LocationState = string.IsNullOrEmpty(metadata.LocationState) ? null : metadata.LocationState;
                        metadata.LocationCity = string.IsNullOrEmpty(metadata.LocationCity) ? null : metadata.LocationCity;
                        metadata.LocationCountry = string.IsNullOrEmpty(metadata.LocationCountry) ? null : metadata.LocationCountry;
                    }
                }
            }
            return metadata;
        }

    }


}
