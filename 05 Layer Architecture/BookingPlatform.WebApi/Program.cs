using BookingPlatform.Authentification;
using BookingPlatform.DataAccess;
using BookingPlatform.DataAccess.Mapper;
using BookingPlatform.DataAccess.Persistence;
using BookingPlatform.Domain;
using BookingPlatform.Domain.Persistence;
using BookingPlatform.Domain.Service;
using BookingPlatform.Domain.Service.DomainException;
using BookingPlatform.WebApi;
using BookingPlatform.WebApi.Configuration;
using BookingPlatform.WebApi.Dto.V2.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

// Build application
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Set up the configuration
builder.Services.Configure<DatabaseConfiguration>(
	builder.Configuration.GetSection("DatabaseConfiguration"));
builder.Services.Configure<AuthOptions>(
	builder.Configuration.GetSection("AuthOptions"));

// Set up the Postgres connection
builder.Services.AddDbContext<BookingContext>((sc, options) =>
{
	var databaseConfigurationOptions = sc.GetService<IOptions<DatabaseConfiguration>>();
	var connectionString = databaseConfigurationOptions?.Value.ConnectionString;
	if (string.IsNullOrEmpty(connectionString))
		throw new System.Exception("DatabaseConfigurationOptions is null");
	options.UseNpgsql(connectionString);
});

// Booking
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingMapper, BookingMapper>();
builder.Services.AddScoped<ITimeProvider, TimeProvider>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Users
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// General
builder.Services
	.AddControllers()
	.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BookingDtoValidator>());

// Auth
var authOptions = new AuthOptions();
builder.Configuration.GetSection(nameof(AuthOptions))
	.Bind(authOptions);
if (new AuthOptionsValidator().Validate(name: string.Empty, authOptions).Failed)
	throw new Exception("Config is not valid");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(op =>
		op.TokenValidationParameters = new TokenValidationParameters
		{
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOptions.SecretKey)),
			ValidateIssuerSigningKey = true,

			ValidIssuer = authOptions.Issuer,
			ValidateIssuer = true,

			ValidateAudience = false,
			// Allow to use seconds for expiration of token
			// Required only when token lifetime less than 5 minutes
			ClockSkew = TimeSpan.Zero
		});

// Commands to add a migration and update the database
// dotnet ef --project ..\BookingPlatform.DataAccess migrations add InitialCreate
// dotnet ef --project ..\BookingPlatform.DataAccess database update

// Middleware
var app = builder
	.Build();

// Custom exception handling
if (app.Environment.IsDevelopment())
	app.UseDeveloperExceptionPage();
/*else
	app.UseExceptionHandler(new ExceptionHandlerOptions
	{
		ExceptionHandler = async ctx =>
		{
			var feature = ctx.Features.Get<IExceptionHandlerFeature>()!;
			if (feature.Error is StatusCodedException statusCodedException)
				ctx.Response.StatusCode = statusCodedException.StatusCode;
			await ctx.Response.WriteAsync(feature.Error.Message);
		}
	});*/

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());

app.Run();
