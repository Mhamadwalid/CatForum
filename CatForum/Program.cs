using Microsoft.EntityFrameworkCore;
using CatForum.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddControllersWithViews(); // Enables MVC pattern with controllers and views

// Configure Entity Framework Core to use SQL Server with a connection string from appsettings.json
builder.Services.AddDbContext<CatForumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatForumContext")));

var app = builder.Build(); // Build the application with the configured services

// Configure middleware for handling HTTP requests
if (!app.Environment.IsDevelopment()) // Check if the app is running in a production-like environment
{
    app.UseExceptionHandler("/Home/Error"); // Redirect users to an error page when an unhandled exception occurs
    app.UseHsts(); // Enforce HTTPS for security
}

app.UseHttpsRedirection(); // Automatically redirect HTTP requests to HTTPS

// Enable serving static files 
app.UseStaticFiles();

app.UseRouting(); // Enable routing, allowing requests to reach the correct controller/action

app.UseAuthorization(); // Enforce authentication and authorization rules if implemented

// Define the default routing pattern for controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default route: HomeController -> Index action

app.Run(); // Start handling requests
