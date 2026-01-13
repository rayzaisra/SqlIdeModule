using SqlIdeModule.Core.Services;
using SqlIdeModule.Web.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SqlIdeModule.Web.Areas.SqlIde.Controllers
{
    public class SqlIdeController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly IDatabaseExplorerService _explorerService;
        private readonly ISqlExecutionService _executionService;

        public SqlIdeController()
        {
            // Use the configured connection string provider
            var connString = SqlIdeConfiguration.GetConnectionString();

            if (string.IsNullOrEmpty(connString))
            {
                throw new InvalidOperationException("Connection string not configured. Please configure SqlIdeConfiguration in Global.asax Application_Start()");
            }

            _authService = new AuthenticationService(connString);
            _explorerService = new DatabaseExplorerService();
            _executionService = new SqlExecutionService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Authenticate(string username, string password)
        {
            try
            {
                var result = _authService.Authenticate(username, password);

                if (result.Success)
                {
                    Session["SqlIdeConnectionString"] = result.ConnectionString;
                    Session["SqlIdeAuthenticated"] = true;
                }

                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Authentication error: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetDatabaseStructure()
        {
            try
            {
                if (Session["SqlIdeAuthenticated"] == null || !(bool)Session["SqlIdeAuthenticated"])
                {
                    return Json(new { success = false, message = "Not authenticated" }, JsonRequestBehavior.AllowGet);
                }

                var connString = Session["SqlIdeConnectionString"] as string;

                var structure = new
                {
                    success = true,
                    database = _explorerService.GetDatabaseName(connString),
                    tables = _explorerService.GetTables(connString),
                    views = _explorerService.GetViews(connString),
                    storedProcedures = _explorerService.GetStoredProcedures(connString),
                    functions = _explorerService.GetFunctions(connString)
                };

                return Json(structure, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error loading database structure: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetObjectDefinition(string objectName, string objectType)
        {
            try
            {
                if (Session["SqlIdeAuthenticated"] == null || !(bool)Session["SqlIdeAuthenticated"])
                {
                    return Json(new { success = false, message = "Not authenticated" });
                }

                var connString = Session["SqlIdeConnectionString"] as string;

                var definition = _explorerService.GetObjectDefinition(connString, objectName, objectType);
                return Json(new { success = true, definition = definition });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error getting object definition: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExecuteQuery(string query)
        {
            try
            {
                // Check authentication
                if (Session["SqlIdeAuthenticated"] == null || !(bool)Session["SqlIdeAuthenticated"])
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Not authenticated. Please log in again.",
                        Columns = new string[0],
                        Rows = new object[0][],
                        RowCount = 0,
                        ExecutionTime = 0
                    });
                }

                // Validate query
                if (string.IsNullOrWhiteSpace(query))
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Query cannot be empty",
                        Columns = new string[0],
                        Rows = new object[0][],
                        RowCount = 0,
                        ExecutionTime = 0
                    });
                }

                var connString = Session["SqlIdeConnectionString"] as string;

                if (string.IsNullOrEmpty(connString))
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Connection string not found in session",
                        Columns = new string[0],
                        Rows = new object[0][],
                        RowCount = 0,
                        ExecutionTime = 0
                    });
                }

                // Execute query
                var result = _executionService.ExecuteQuery(connString, query);

                // Ensure all properties are set
                if (result.Columns == null) result.Columns = new string[0];
                if (result.Rows == null) result.Rows = new object[0][];

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Unexpected error: " + ex.Message + " | Stack: " + ex.StackTrace,
                    Columns = new string[0],
                    Rows = new object[0][],
                    RowCount = 0,
                    ExecutionTime = 0
                });
            }
        }

        [HttpPost]
        public JsonResult Logout()
        {
            try
            {
                Session["SqlIdeConnectionString"] = null;
                Session["SqlIdeAuthenticated"] = null;

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
