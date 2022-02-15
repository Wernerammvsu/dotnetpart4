using EFCoreExample.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDbExample.Configuration;

// Build application
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services.Configure<DatabaseConfiguration>(
	builder.Configuration.GetSection("DatabaseConfiguration"));

builder.Services.AddControllers();

builder.Services.AddDbContext<BookingContext>((sc, options) =>
	options.UseNpgsql(sc.GetService<IOptions<DatabaseConfiguration>>().Value.ConnectionString));

// Commands to add a migration and update the database
// dotnet ef migrations add InitialCreate
// dotnet ef database update

// Middleware
var app = builder
	.Build();

app.UseRouting();
app.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());

app.Run();
