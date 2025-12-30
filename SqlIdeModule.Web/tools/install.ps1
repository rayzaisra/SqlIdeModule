param($installPath, $toolsPath, $package, $project)

# Register the MVC Area
$areaRegistrationCode = @"
using System.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SqlIdeModule.Web.Areas.SqlIde.SqlIdeAreaRegistration), "RegisterArea")]

namespace SqlIdeModule.Web.Areas.SqlIde
{
    public partial class SqlIdeAreaRegistration
    {
        public static void RegisterArea()
        {
            AreaRegistration.RegisterAllAreas();
        }
    }
}
"@

# Add WebActivatorEx if not present
$webActivatorPackage = $project.Object.References | Where-Object { $_.Name -eq "WebActivatorEx" }
if (-not $webActivatorPackage) {
    Install-Package WebActivatorEx -ProjectName $project.Name
}

Write-Host "SqlIdeModule installed successfully!"
Write-Host "Access the SQL IDE at: /SqlIde/SqlIde"
Write-Host "Make sure you have a 'DefaultConnection' connection string in Web.config"