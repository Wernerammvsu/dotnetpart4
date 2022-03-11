using DtoValidation.Configuration;
using DtoValidation.DataAccess;
using DtoValidation.Dto.Mapping;
using DtoValidation.Dto.V2.Validation;
using DtoValidation.Service;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// Build application
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Set up the configuration
builder.Services.Configure<DatabaseConfiguration>(
	builder.Configuration.GetSection("DatabaseConfiguration"));

// Set up the Postgres connection
builder.Services.AddDbContext<BookingContext>((sc, options) =>
{
	var databaseConfigurationOptions = sc.GetService<IOptions<DatabaseConfiguration>>();
	var connectionString = databaseConfigurationOptions?.Value.ConnectionString;
	if (string.IsNullOrEmpty(connectionString))
		throw new System.Exception("DatabaseConfigurationOptions is null");
	options.UseNpgsql(connectionString);
});

builder.Services.AddAutoMapper(typeof(DefaultAutoMapperProfile));
builder.Services
	.AddScoped<BookingService>();
builder.Services
	.AddControllers()
	.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BookingDtoValidator>());

// Commands to add a migration and update the database
// dotnet ef migrations add InitialCreate
// dotnet ef database update

// Middleware
var app = builder
	.Build();

app.UseRouting();
app.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());

app.Run();
