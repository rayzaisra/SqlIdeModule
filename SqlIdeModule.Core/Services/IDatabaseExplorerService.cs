using SqlIdeModule.Core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Services
{
    public interface IDatabaseExplorerService
    {
        string GetDatabaseName(string connectionString);
        List<DatabaseObject> GetTables(string connectionString);
        List<DatabaseObject> GetViews(string connectionString);
        List<DatabaseObject> GetStoredProcedures(string connectionString);
        List<DatabaseObject> GetFunctions(string connectionString);
        string GetObjectDefinition(string connectionString, string objectName, string objectType);
    }
}
