#define MonoSqlite
#define noMicrosoftDataSqlite

#if MonoSqlite
using Mono.Data.Sqlite;
#elif MicrosoftDataSqlite
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif


using NLog;
using System;

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

        public int ExecuteNonQuery()
        {
            try
            {
                return databaseCommand.ExecuteNonQuery();
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
            return new CommonSqliteDataReader(databaseCommand.ExecuteReader());
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
