#if MonoSqlite
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
#endif


namespace SqliteDatabase
{
    public class CommonDatabaseTransaction
    {
        public const System.Data.IsolationLevel TransactionReadCommitted = System.Data.IsolationLevel.ReadCommitted;
        public const System.Data.IsolationLevel TransactionSerializable = System.Data.IsolationLevel.Serializable;
        public const System.Data.IsolationLevel TransactionNull = System.Data.IsolationLevel.Serializable;

#if MonoSqlite
        private Mono.Data.Sqlite.SqliteTransaction databaseTransaction;
        public SqliteTransaction DatabaseTransaction { get => databaseTransaction; set => databaseTransaction = value; }

        public CommonDatabaseTransaction(Mono.Data.Sqlite.SqliteTransaction sqliteTransaction)
        {
            databaseTransaction = sqliteTransaction;
        }
#else
        private System.Data.SQLite.SQLiteTransaction databaseTransaction;
        public SQLiteTransaction DatabaseTransaction { get => databaseTransaction; set => databaseTransaction = value; }
        public CommonDatabaseTransaction (System.Data.SQLite.SQLiteTransaction sqliteTransaction)
        {
            databaseTransaction = sqliteTransaction;
        }
#endif

    }
}
