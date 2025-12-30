using SqlIdeModule.Core.Models;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Services
{
    public class SqlExecutionService : ISqlExecutionService
    {
        private readonly int _commandTimeout = 30;

        public QueryResult ExecuteQuery(string connectionString, string query)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Security: Block dangerous commands
                if (ContainsDangerousCommand(query))
                {
                    return new QueryResult
                    {
                        Success = false,
                        Message = "Query contains restricted commands (DROP, TRUNCATE, etc.)"
                    };
                }

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandTimeout = _commandTimeout;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        var columns = new List<string>();
                        var rows = new List<object[]>();

                        // Get column names
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columns.Add(reader.GetName(i));
                        }

                        // Get rows
                        while (reader.Read())
                        {
                            var row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            rows.Add(row);
                        }

                        stopwatch.Stop();

                        return new QueryResult
                        {
                            Success = true,
                            Message = $"{rows.Count} row(s) returned",
                            Columns = columns.ToArray(),
                            Rows = rows.ToArray(),
                            RowCount = rows.Count,
                            ExecutionTime = stopwatch.Elapsed.TotalSeconds
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                stopwatch.Stop();
                return new QueryResult
                {
                    Success = false,
                    Message = $"SQL Error: {ex.Message}",
                    ExecutionTime = stopwatch.Elapsed.TotalSeconds
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new QueryResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    ExecutionTime = stopwatch.Elapsed.TotalSeconds
                };
            }
        }


        private bool ContainsDangerousCommand(string query)
        {
            var dangerous = new[] { "DROP ", "TRUNCATE ", "DELETE ", "GRANT ", "REVOKE " };
            var upperQuery = query.ToUpper();

            foreach (var cmd in dangerous)
            {
                if (upperQuery.Contains(cmd))
                    return true;
            }

            return false;
        }
    }
}
