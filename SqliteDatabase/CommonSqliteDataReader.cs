#define MonoSqlite
#define noMicrosoftDataSqlite

#if MonoSqlite
using Mono.Data.Sqlite;
#elif MicrosoftDataSqlite
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif


using System;

namespace SqliteDatabase
{
    public class CommonSqliteDataReader : IDisposable
    {
        #region public CommonSqliteDataReader
#if MonoSqlite
        Mono.Data.Sqlite.SqliteDataReader sqliteDataReader;
        public CommonSqliteDataReader(SqliteDataReader sqliteDataReader)
#elif MicrosoftDataSqlite
        SqliteDataReader sqliteDataReader;
        public CommonSqliteDataReader(SqliteDataReader sqliteDataReader)
#else
        SQLiteDataReader sqliteDataReader;
        public CommonSqliteDataReader(SQLiteDataReader sqliteDataReader)
#endif 
        {
            this.sqliteDataReader = sqliteDataReader;
        }
        #endregion

        #region Read
        public bool Read()
        {
            try
            {
                return this.sqliteDataReader.Read();
            } catch 
            {
                return false;
            }
        }
        #endregion

        #region 
        public object this[string key] => sqliteDataReader[key];

        public object this[int i] => sqliteDataReader[i];
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                if (sqliteDataReader != null)sqliteDataReader.Dispose();
                disposedValue = true;
            }
        }
        #endregion

        #region Destructor
        ~CommonSqliteDataReader()
        {
            Dispose(false);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
