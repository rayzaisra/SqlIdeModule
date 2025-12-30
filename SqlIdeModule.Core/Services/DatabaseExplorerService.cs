using SqlIdeModule.Core.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Services
{
    public class DatabaseExplorerService : IDatabaseExplorerService
    {
        public string GetDatabaseName(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            return builder.InitialCatalog;
        }

        public List<DatabaseObject> GetTables(string connectionString)
        {
            return GetDatabaseObjects(connectionString, "SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME");
        }

        public List<DatabaseObject> GetViews(string connectionString)
        {
            return GetDatabaseObjects(connectionString, "SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS ORDER BY TABLE_NAME");
        }

        public List<DatabaseObject> GetStoredProcedures(string connectionString)
        {
            return GetDatabaseObjects(connectionString,
                @"SELECT SCHEMA_NAME(schema_id) AS TABLE_SCHEMA, name AS TABLE_NAME 
                  FROM sys.procedures 
                  WHERE is_ms_shipped = 0 
                  ORDER BY name");
        }

        public List<DatabaseObject> GetFunctions(string connectionString)
        {
            return GetDatabaseObjects(connectionString,
                @"SELECT SCHEMA_NAME(schema_id) AS TABLE_SCHEMA, name AS TABLE_NAME 
                  FROM sys.objects 
                  WHERE type IN ('FN', 'IF', 'TF') 
                  ORDER BY name");
        }

        private List<DatabaseObject> GetDatabaseObjects(string connectionString, string query)
        {
            var objects = new List<DatabaseObject>();

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        objects.Add(new DatabaseObject
                        {
                            Schema = reader["TABLE_SCHEMA"].ToString(),
                            Name = reader["TABLE_NAME"].ToString()
                        });
                    }
                }
            }

            return objects;
        }

        public string GetObjectDefinition(string connectionString, string objectName, string objectType)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_helptext", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@objname", objectName);

                conn.Open();
                var definition = new StringBuilder();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        definition.Append(reader[0].ToString());
                    }
                }

                return definition.ToString();
            }
        }
    }
}
