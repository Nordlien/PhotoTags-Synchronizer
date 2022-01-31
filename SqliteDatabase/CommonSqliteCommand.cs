#define MonoSqlite
#define noMicrosoftDataSqlite
#if DEBUG
#define DEBUGSCAN
#endif

#if MonoSqlite
using Mono.Data.Sqlite;
#elif MicrosoftDataSqlite
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif


using NLog;
using System;
using System.Diagnostics;

namespace SqliteDatabase
{
    public class CommonSqliteCommand : IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

#if MonoSqlite
        private SqliteCommand databaseCommand;
        public SqliteCommand DatabaseCommand { get => databaseCommand; set => databaseCommand = value; }

        public CommonSqliteCommand(string commandText, SqliteConnection connection, SqliteTransaction transaction)
        {
            databaseCommand = new SqliteCommand(commandText, connection, transaction);            
        }

        public CommonSqliteCommand(string commandText, SqliteConnection connection)
        {
            databaseCommand = new SqliteCommand(commandText, connection);
        }
#elif MicrosoftDataSqlite
        private SqliteCommand databaseCommand;
        public SqliteCommand DatabaseCommand { get => databaseCommand; set => databaseCommand = value; }

        public CommonSqliteCommand(string commandText, SqliteConnection connection)
        {
            databaseCommand = new SqliteCommand(commandText, connection);
        }

        public CommonSqliteCommand(string commandText, SqliteConnection connection, SqliteTransaction transaction)
        {
            databaseCommand = new SqliteCommand(commandText, connection, transaction);
        }
#else
        private SQLiteCommand databaseCommand;
        public SQLiteCommand DatabaseCommand { get => databaseCommand; set => databaseCommand = value; }

        public CommonSqliteCommand(string commandText, SQLiteConnection connection)
        {
                databaseCommand = new SQLiteCommand(commandText, connection);
        }

        public CommonSqliteCommand(string commandText, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            databaseCommand = new SQLiteCommand(commandText, connection, transaction);
        }
#endif


#if DEBUGSCAN
        private void TableScanDebug(int debugTickCount, SqliteCommand databaseCommand)
        {
            #region DEBUG
            Mono.Data.Sqlite.SqliteCommand sqliteCommand = new SqliteCommand("EXPLAIN QUERY PLAN " + databaseCommand.CommandText, databaseCommand.Connection);
            foreach (SqliteParameter sqliteParameter in databaseCommand.Parameters)
            {
                sqliteCommand.Parameters.Add(sqliteParameter);
            }
            SqliteDataReader sqliteDataReaderDebug = sqliteCommand.ExecuteReader();
            if (sqliteDataReaderDebug.Read())
            {
                var detail = sqliteDataReaderDebug[3];
                if (!detail.ToString().StartsWith("SEARCH", StringComparison.InvariantCultureIgnoreCase))
                {
                    Debug.WriteLine("0 - Sql Performance using SCAN");
                    Debug.WriteLine("0 - Sql Performance using SCAN Command: " + databaseCommand.CommandText);
                    Debug.WriteLine("0 - Sql Performance using SCAN Index: " + detail.ToString());
                    
                } else
                {
                    if (detail.ToString().Contains("sqlite_autoindex_MediaMetadata_1"))
                    {
                        if (!detail.ToString().Contains("(Broker=? AND FileDirectory=? AND FileName=? AND FileDateModified=?)") &&
                            !detail.ToString().Contains("(Broker=? AND FileDirectory=? AND FileName=?)") &&
                            !detail.ToString().Contains("(Broker=? AND FileDirectory=?)") &&                            
                            !databaseCommand.CommandText.Contains("SELECT DISTINCT PersonalAlbum FROM MediaMetadata WHERE Broker = @Broker") &&
                            !databaseCommand.CommandText.Contains("SELECT DISTINCT PersonalComments FROM MediaMetadata WHERE Broker = @Broke") &&
                            !databaseCommand.CommandText.Contains("SELECT DISTINCT PersonalDescription FROM MediaMetadata WHERE Broker = @Broker") &&
                            !databaseCommand.CommandText.Contains("SELECT DISTINCT PersonalTitle FROM MediaMetadata WHERE Broker = @Broker") &&
                            !databaseCommand.CommandText.Contains("SELECT DISTINCT LocationName FROM MediaMetadata WHERE Broker = @Broker") &&
                            !databaseCommand.CommandText.Contains("SELECT DISTINCT LocationCity FROM MediaMetadata WHERE Broker = @Broke") &&
                            !databaseCommand.CommandText.Contains("SELECT DISTINCT LocationState FROM MediaMetadata WHERE Broker = @Broker") &&
                            !databaseCommand.CommandText.Contains("SELECT DISTINCT LocationCountry FROM MediaMetadata WHERE Broker = @Broker")
                            )
                        {
                            Debug.WriteLine("1 - Sql Performance NOT use full Index, Command: " + databaseCommand.CommandText);
                            Debug.WriteLine("1 - Sql Performance NOT use full Index: " + detail.ToString());
                        }
                    }
                    else if (detail.ToString().Contains("sqlite_autoindex_MediaPersonalKeywords_1"))
                    {
                        if (!detail.ToString().Contains("(Broker=? AND FileDirectory=? AND FileName=? AND FileDateModified=?)") &&
                            !detail.ToString().Contains("(Broker=? AND FileDirectory=?)"))
                        {
                            Debug.WriteLine("2 - Sql Performance NOT use full Index, Command: " + databaseCommand.CommandText);
                            Debug.WriteLine("2 - Sql Performance NOT use full Index: " + detail.ToString());
                        }
                    }
                    else if (detail.ToString().Contains("sqlite_autoindex_MediaPersonalRegions_1"))
                    {
                        if (!detail.ToString().Contains("(Broker=? AND FileDirectory=? AND FileName=? AND FileDateModified=? AND Type=? AND Name=?") &&
                            !detail.ToString().Contains("(Broker=? AND FileDirectory=? AND FileName=? AND FileDateModified=?)") &&
                            !detail.ToString().Contains("(Broker=? AND FileDirectory=?)") &&
                            !databaseCommand.CommandText.Contains("SELECT Name, Count(1) AS CountNames FROM MediaPersonalRegions WHERE Broker = @Broker GROUP BY Name"))
                        {
                            Debug.WriteLine("3 - Sql Performance NOT use full Index, Command: " + databaseCommand.CommandText);
                            Debug.WriteLine("3 - Sql Performance NOT use full Index: " + detail.ToString());
                        }
                    }                                  
                    else if (detail.ToString().Contains("sqlite_autoindex_MediaThumbnail_1"))
                    {
                        if (!detail.ToString().Contains("(FileDirectory=? AND FileName=? AND FileDateModified=?)") &&
                            !detail.ToString().Contains("(FileDirectory=?)"))
                        {
                            Debug.WriteLine("4 - Sql Performance NOT use full Index: " + databaseCommand.CommandText);
                            Debug.WriteLine("4 - Sql Performance NOT use full Index: " + detail.ToString());
                        }
                    }
                    else if (detail.ToString().Contains("sqlite_autoindex_MediaExiftoolTagsWarning_1"))
                    {
                        if (!detail.ToString().Contains("(FileDirectory=? AND FileName=? AND FileDateModified=? AND OldRegion=? AND OldCommand=? AND NewRegion=? AND NewCommand=?)") &&
                            !detail.ToString().Contains("(FileDirectory=? AND FileName=? AND FileDateModified=?)")
                            )
                        {
                            Debug.WriteLine("4 - Sql Performance NOT use full Index: " + databaseCommand.CommandText);
                            Debug.WriteLine("4 - Sql Performance NOT use full Index: " + detail.ToString());
                        }
                    }
                    else if (detail.ToString().Contains("sqlite_autoindex_MediaExiftoolTags_1"))
                    {
                        if (!detail.ToString().Contains("(FileDirectory=? AND FileName=? AND FileDateModified=?)")) 
                        {
                            Debug.WriteLine("4 - Sql Performance NOT use full Index: " + databaseCommand.CommandText);
                            Debug.WriteLine("4 - Sql Performance NOT use full Index: " + detail.ToString());
                        }
                    }
                    else if (detail.ToString().Contains("MediaPersonalRegions_Name_FileDateModified"))
                    {
                        //SELECT Thumbnail FROM MediaPersonalRegions WHERE Name = @Name AND Thumbnail IS NOT NULL ORDER BY FileDateModified LIMIT 1
                        if (!detail.ToString().Contains("(Name=?)"))
                        {
                            Debug.WriteLine("4 - Sql Performance NOT use full Index, Command: " + databaseCommand.CommandText);
                            Debug.WriteLine("4 - Sql Performance NOT use full Index: " + detail.ToString());
                        }
                    }
                    else
                    {
                        Debug.WriteLine("99 - Table need to be checked, Command: " + databaseCommand.CommandText);
                        Debug.WriteLine("99 - Table need to be checked, Index: " + detail.ToString());
                    }
                    //DEBUG SCAN
                }
            }

            int timerTicksDiff = Environment.TickCount - debugTickCount;
            if (timerTicksDiff > 100)
            {
                Debug.WriteLine("100 - Sql Performance slow: " + timerTicksDiff);
                Debug.WriteLine("100 - Sql Performance command: " + databaseCommand.CommandText);
                //DEBUG - Table scan, To Slow Performance???
            }
            #endregion
        }
#endif
        public int ExecuteNonQuery()
        {
            try
            {
#if DEBUGSCAN
                int debugTickCount = Environment.TickCount;
#endif
                if (databaseCommand.Connection.State != System.Data.ConnectionState.Open) return 0;
                int result = databaseCommand.ExecuteNonQuery();

#if DEBUGSCAN
                TableScanDebug(debugTickCount, databaseCommand);
#endif
                return result;
            }            
            catch (Exception ex)
            {
                string databaseCommandText = databaseCommand.CommandText;
                foreach (SqliteParameter sqliteParameter in databaseCommand.Parameters)
                    databaseCommandText = databaseCommandText.Replace(sqliteParameter.ParameterName, 
                            (sqliteParameter.Value == null ? 
                            "null" :
                                (
                                    (sqliteParameter.DbType == System.Data.DbType.String ? "'" : "") + 
                                    sqliteParameter.Value.ToString() + 
                                    (sqliteParameter.DbType == System.Data.DbType.String ? "'" : "")
                                )
                            )
                        );
                Logger.Error(ex, "Database error, Sql command:" + databaseCommandText);
                return -1;
            }
        }

        public void Dispose()
        {
            databaseCommand.Dispose();
        }

#if MonoSqlite
        public SqliteParameterCollection Parameters { get => databaseCommand.Parameters; }
#elif MicrosoftDataSqlite
        public SqliteParameterCollection Parameters { get => databaseCommand.Parameters; }
#else
        public SQLiteParameterCollection Parameters { get => databaseCommand.Parameters; }
#endif
        public CommonSqliteDataReader ExecuteReader()
        {
            CommonSqliteDataReader sqliteDataReader = null;
#if DEBUGSCAN
            int debugTickCount = Environment.TickCount;
#endif
            if (databaseCommand.Connection.State == System.Data.ConnectionState.Open) 
                sqliteDataReader = new CommonSqliteDataReader(databaseCommand.ExecuteReader());
#if DEBUGSCAN
            TableScanDebug(debugTickCount, databaseCommand);
#endif
            return sqliteDataReader;
        }

        public object ExecuteScalar()
        {
            return databaseCommand.ExecuteScalar();
        }

        public void Prepare()
        {
            //databaseCommand.Prepare();
        }
    }
}
