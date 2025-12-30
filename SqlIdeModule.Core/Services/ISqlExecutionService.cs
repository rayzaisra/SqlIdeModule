using SqlIdeModule.Core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Services
{
    public interface ISqlExecutionService
    {
        QueryResult ExecuteQuery(string connectionString, string query);
    }
}
