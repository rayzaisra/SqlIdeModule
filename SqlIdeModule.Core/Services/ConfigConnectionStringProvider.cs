using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Services
{
    public class ConfigConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _connectionStringName;

        public ConfigConnectionStringProvider(string connectionStringName = "DefaultConnection")
        {
            _connectionStringName = connectionStringName;
        }

        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[_connectionStringName]?.ConnectionString;
        }
    }
}
