using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Models
{
    public class SqlCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public bool IntegratedSecurity { get; set; }
    }

    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ConnectionString { get; set; }
    }

    public class DatabaseObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Schema { get; set; }
    }

    public class QueryResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Columns { get; set; }
        public object[][] Rows { get; set; }
        public int RowCount { get; set; }
        public double ExecutionTime { get; set; }
    }
}
