using Booking.DataAccess;
using Booking.DataAccess.Mapper;
using Booking.Domain;
using Booking.Domain.Persistence;
using Booking.Domain.Service;
using Booking.WebApi;
using Booking.WebApi.Configuration;
using Booking.WebApi.Dto.V2.Validation;
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

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingMapper, BookingMapper>();
builder.Services.AddScoped<ITimeProvider, TimeProvider>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services
	.AddControllers()
	.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BookingDtoValidator>());

// Commands to add a migration and update the database
// dotnet ef --project ..\Booking.DataAccess migrations add InitialCreate
// dotnet ef --project ..\Booking.DataAccess database update

// Middleware
var app = builder
	.Build();

app.UseRouting();
app.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());

app.Run();
