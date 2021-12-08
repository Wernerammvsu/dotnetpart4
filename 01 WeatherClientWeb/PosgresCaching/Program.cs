using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PostgresCaching.Caching;

// Build application
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<PosgresDictionary>();

// Middleware
var app = builder
	.Build();

app.UseRouting();
app.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());

app.Run();
