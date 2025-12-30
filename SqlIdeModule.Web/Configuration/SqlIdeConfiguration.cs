using SqlIdeModule.Core.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Web.Configuration
{
    public static class SqlIdeConfiguration
    {
        private static IConnectionStringProvider _connectionStringProvider;

        /// <summary>
        /// Configure SQL IDE to use Web.config connection string (default)
        /// </summary>
        public static void UseWebConfig(string connectionStringName = "DefaultConnection")
        {
            _connectionStringProvider = new ConfigConnectionStringProvider(connectionStringName);
        }

        /// <summary>
        /// Configure SQL IDE to use a custom connection string builder function
        /// Example: SqlIdeConfiguration.UseCustomConnectionString(() => YourClass.ConnectionStringBuilder.Construct());
        /// </summary>
        public static void UseCustomConnectionString(Func<string> connectionStringBuilder)
        {
            _connectionStringProvider = new CustomConnectionStringProvider(connectionStringBuilder);
        }

        /// <summary>
        /// Configure SQL IDE to use a static connection string
        /// </summary>
        public static void UseStaticConnectionString(string connectionString)
        {
            _connectionStringProvider = new CustomConnectionStringProvider(() => connectionString);
        }

        /// <summary>
        /// Get the configured connection string provider
        /// </summary>
        internal static IConnectionStringProvider GetConnectionStringProvider()
        {
            // Default to Web.config if not configured
            if (_connectionStringProvider == null)
            {
                _connectionStringProvider = new ConfigConnectionStringProvider();
            }

            return _connectionStringProvider;
        }

        /// <summary>
        /// Get the actual connection string
        /// </summary>
        internal static string GetConnectionString()
        {
            return GetConnectionStringProvider().GetConnectionString();
        }
    }
}
