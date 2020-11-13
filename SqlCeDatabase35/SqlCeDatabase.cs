using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ChristianHelle.DatabaseTools.SqlCe
{
    public class SqlCeDatabase : ISqlCeDatabase
    {
        public SqlCeDatabase()
        {
        }


        public object ExecuteQuery(string query, StringBuilder errors, StringBuilder messages, out int resultCount)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                using (var conn = new SqlCeConnection(ConnectionString))
                {
                    conn.InfoMessage += (sender, e) =>
                    {
                        messages.AppendLine(e.Message);
                        foreach (SqlCeError error in e.Errors)
                            errors.AppendLine(error.ToString());
                    };

                    int affectedRows = 0;
                    var split = query.Split(new[] { ";" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    var tables = new DataSet();
                    using (var command = conn.CreateCommand())
                    {
                        conn.Open();
                        foreach (var sql in split)
                        {
                            try
                            {
                                if (sql.Trim().StartsWith("select", StringComparison.InvariantCultureIgnoreCase))
                                {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                                    using (var adapter = new SqlCeDataAdapter(sql, conn))
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                                    {
                                        var table = new DataTable();
                                        affectedRows += adapter.Fill(table);
                                        tables.Tables.Add(table);
                                        messages.AppendLine(string.Format("Retrieved {0} row(s)", table.Rows.Count));
                                    }
                                }
                                else
                                {
                                    command.CommandText = sql;
                                    affectedRows += command.ExecuteNonQuery();
                                }
                            }
                            catch (SqlCeException e)
                            {
                                foreach (SqlCeError error in e.Errors)
                                    errors.AppendLine(error.Message);
                            }
                        }
                        messages.AppendLine();
                        messages.AppendLine(string.Format("Total affected row(s): {0}", affectedRows));

                        resultCount = affectedRows;
                        return tables;
                    }
                }
            }
            catch (SqlCeException e)
            {
                foreach (SqlCeError error in e.Errors)
                    errors.AppendLine(error.Message);
            }
            catch (Exception e)
            {
                errors.AppendLine(e.Message);
            }
            finally
            {
                messages.AppendLine();
                messages.AppendLine("Executed in " + stopwatch.Elapsed);
            }

            resultCount = 0;
            return null;
        }

        public void CreateDatabase(string filename, string password, int? maxDatabaseSize)
        {
            if (filename == null) 
                throw new ArgumentNullException("filename");

            var connStr = "Data Source=" + filename;
            if (!string.IsNullOrWhiteSpace(password))
                connStr += "; Password=" + password;
            if (maxDatabaseSize.HasValue)
                connStr += "; Max Database Size=" + maxDatabaseSize.Value;

            using (var engine = new SqlCeEngine(connStr))
                engine.CreateDatabase();
        }

        public string ConnectionString { get; set; }

     

    }
}
