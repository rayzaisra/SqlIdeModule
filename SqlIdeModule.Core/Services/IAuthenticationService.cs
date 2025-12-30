using SqlIdeModule.Core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Services
{
    public interface IAuthenticationService
    {
        AuthenticationResult Authenticate(string username, string password);
        SqlCredentials GetCredentialsFromConnectionString(string connectionString);
    }
}
