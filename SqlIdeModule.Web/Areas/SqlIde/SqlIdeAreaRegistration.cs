using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SqlIdeModule.Web.Areas.SqlIde
{
    public class SqlIdeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "SqlIde"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SqlIde_default",
                "SqlIde/{controller}/{action}/{id}",
                new { controller = "SqlIde", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
