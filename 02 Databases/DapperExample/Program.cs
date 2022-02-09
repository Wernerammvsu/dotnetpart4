using DapperExample.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// Build application
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<UserRepository>();

// Commands to add a migration and update the database
// dotnet ef migrations add InitialCreate
// dotnet ef database update

// Middleware
var app = builder
	.Build();

app.UseRouting();
app.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());

app.Run();
