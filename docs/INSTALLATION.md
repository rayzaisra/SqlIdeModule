📦 Installation Guide – SQL IDE Module

This document explains how to install and set up SQL IDE Module for ASP.NET MVC in your project.

✅ Prerequisites

Before installing, make sure you have:

ASP.NET MVC targeting .NET Framework 4.5.2 or higher

SQL Server (any version)

Visual Studio 2017+ or Visual Studio 2022

NuGet Package Manager available

📥 Installation Methods
1️⃣ Install via NuGet Package Manager Console (Recommended)

Open Package Manager Console and run:

Install-Package SqlIdeModule

2️⃣ Install via NuGet Package Manager UI

Right-click your MVC project

Select Manage NuGet Packages

Search for SqlIdeModule

Click Install

3️⃣ Install from Local NuGet Source (Development / Offline)

If you built the .nupkg locally:

Install-Package SqlIdeModule -Source "C:\path\to\nupkg\folder"

⚙️ Post-Installation Setup
Step 1: Configure Connection String

Open Global.asax.cs and add one of the following options.

Option A – Use Web.config (Simple)
using SqlIdeModule.Web.Configuration;

protected void Application_Start()
{
    AreaRegistration.RegisterAllAreas();
    SqlIdeConfiguration.UseWebConfig("DefaultConnection");
}


Then add to Web.config:

<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Server=localhost;Database=YourDB;User Id=sa;Password=yourpassword;"
       providerName="System.Data.SqlClient" />
</connectionStrings>

Option B – Custom Connection Builder (Recommended for Production)
SqlIdeConfiguration.UseCustomConnectionString(() =>
    YourNamespace.ConnectionStringBuilder.Construct()
);


Example builder:

public static class ConnectionStringBuilder
{
    public static string Construct()
    {
        return new SqlConnectionStringBuilder
        {
            DataSource = "localhost",
            InitialCatalog = "YourDB",
            UserID = "user",
            Password = "password"
        }.ConnectionString;
    }
}

Option C – Static Connection String (Testing Only)
SqlIdeConfiguration.UseStaticConnectionString(
    "Server=localhost;Database=TestDB;User Id=test;Password=test;"
);

🔗 Optional: Add Navigation Menu

In _Layout.cshtml:

<li>
  @Html.ActionLink("SQL IDE", "Index", "SqlIde", new { area = "SqlIde" }, null)
</li>

🌐 Accessing the SQL IDE

After running the application, navigate to:

http://localhost:{port}/SqlIde/SqlIde


You should see the SQL IDE login screen.

🔐 Security Notes (Important)

Use read-only SQL users in production

Protect the module with [Authorize]

Disable in production if not required

Example:

[Authorize(Roles = "Administrator,Developer")]
public class SqlIdeController : Controller
{
}

🧪 Verify Installation Checklist

✅ NuGet package installed
✅ Area SqlIde registered
✅ Connection string configured
✅ Page loads at /SqlIde/SqlIde
✅ Query execution works

🆘 Troubleshooting
❌ Page not found

Ensure AreaRegistration.RegisterAllAreas() is called

Rebuild solution

❌ Login fails

Verify SQL credentials

Check firewall / SQL Server auth mode

❌ NuGet install issues

Clear NuGet cache:

nuget locals all -clear

📚 Next Steps

See README.md for usage & features

Review SECURITY section before production use

Customize query limits and timeout if needed

📄 License

This project is licensed under the MIT License.
See LICENSE for details.