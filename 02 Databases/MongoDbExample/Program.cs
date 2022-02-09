using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDbExample.Configuration;
using MongoDbExample.DataAccess;

// Build application
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var bookingDatabaseConfiguration = new BookingDatabaseConfiguration(
	connectionString: "mongodb://root:example@localhost:27017/",
	databaseName: "Booking");
builder.Services.AddSingleton<BookingDatabaseConfiguration>(_ => bookingDatabaseConfiguration);
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<BookingRespository>();

// Middleware
var app = builder
	.Build();

app.UseRouting();
app.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());

app.Run();
