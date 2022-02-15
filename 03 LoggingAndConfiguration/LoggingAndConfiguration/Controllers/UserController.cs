using EFCoreExample.DataAccess;
using EFCoreExample.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDbExample.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreExample.Controllers
{
	[Route("user")]
	public class UserController : Controller
	{
		private readonly BookingContext _bookingContext;
		private readonly IOptionsMonitor<DatabaseConfiguration> _databaseConfiguration;
		private readonly ILogger<UserController> _logger;

		private readonly IDisposable _congifChangedListener;

		public UserController(BookingContext bookingContext,
			IOptionsMonitor<DatabaseConfiguration> databaseConfiguration,
			ILogger<UserController> logger)
		{
			_bookingContext = bookingContext;
			_databaseConfiguration = databaseConfiguration;
			_logger = logger;

			_congifChangedListener = databaseConfiguration.OnChange(LogWarning);
		}

		private void LogWarning(DatabaseConfiguration arg1, string arg2)
		{
			_logger.LogWarning("Something has changed {arg1}, {arg2}", arg1, arg2);
		}

		public async Task<ActionResult<string[]>> GetAllUserNames()
		{
			return await _bookingContext.Users
				.Select(u => u.UserName)
				.ToArrayAsync();
		}

		[HttpPost]
		public async Task<ActionResult<User>> CreateUser([FromBody]User user)
		{
			User userInDb = await _bookingContext
				.Users
				.FirstOrDefaultAsync(u => u.UserName == user.UserName);

			if (userInDb != null)
				return Conflict("User with this name already exists");

			_bookingContext.Users.Add(user);
			await _bookingContext.SaveChangesAsync();

			return Ok(user);
		}
	}
}
