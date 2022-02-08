using EFCoreExample.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// Build application
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<BookingContext>(options =>
	options.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=mysecretpassword"));

// Middleware
var app = builder
	.Build();

app.UseRouting();
app.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());

app.Run();
