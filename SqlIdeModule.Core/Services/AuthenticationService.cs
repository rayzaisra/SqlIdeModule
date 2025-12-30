using SqlIdeModule.Core.Models;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string _baseConnectionString;

        public AuthenticationService(string baseConnectionString)
        {
            _baseConnectionString = baseConnectionString;
        }

        public AuthenticationResult Authenticate(string username, string password)
        {
            try
            {
                var credentials = GetCredentialsFromConnectionString(_baseConnectionString);

                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = credentials.Server,
                    InitialCatalog = credentials.Database,
                    UserID = username,
                    Password = password,
                    IntegratedSecurity = false,
                    ConnectTimeout = 5
                };

                using (var conn = new SqlConnection(builder.ConnectionString))
                {
                    conn.Open();
                    return new AuthenticationResult
                    {
                        Success = true,
                        Message = "Authentication successful",
                        ConnectionString = builder.ConnectionString
                    };
                }
            }
            catch (SqlException ex)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = $"Authentication failed: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public SqlCredentials GetCredentialsFromConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            return new SqlCredentials
            {
                Server = builder.DataSource,
                Database = builder.InitialCatalog,
                Username = builder.UserID,
                Password = builder.Password,
                IntegratedSecurity = builder.IntegratedSecurity
            };
        }
    }
}
