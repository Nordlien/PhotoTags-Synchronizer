using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe
{
    public interface ISqlCeDatabase
    {
        string ConnectionString { get; set; }
        
        object ExecuteQuery(string query, StringBuilder errors, StringBuilder messages, out int resultCount);
        void CreateDatabase(string filename, string password, int? maxDatabaseSize);
    }
}
