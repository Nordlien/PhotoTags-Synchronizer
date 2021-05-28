#if MonoSqlite
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

using System;

namespace SqliteDatabase
{
    public class CommonSqliteDataReader : IDisposable
    {

#if MonoSqlite
        Mono.Data.Sqlite.SqliteDataReader sqliteDataReader;
        public CommonSqliteDataReader(SqliteDataReader sqliteDataReader)
#else
        SQLiteDataReader sqliteDataReader;
        public CommonSqliteDataReader(SQLiteDataReader sqliteDataReader)
#endif 
        {
            this.sqliteDataReader = sqliteDataReader;
        }

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

        public object this[string key]
        {
            get => sqliteDataReader[key];
        }

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

        ~CommonSqliteDataReader()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
