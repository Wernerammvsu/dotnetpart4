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
		private readonly ILogger<UserController> _logger;
		private readonly AppConfiguration _appConfiguration;

		public UserController(BookingContext bookingContext,
			ILogger<UserController> logger, 
			IOptions<AppConfiguration> options)
		{
			_bookingContext = bookingContext;
			_logger = logger;
			_appConfiguration = options.Value;
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
			if (user.UserName.Length < _appConfiguration.UserNameMinLength)
			{
				_logger.LogWarning("Provided username is less than {UserNameMinLength}", _appConfiguration.UserNameMinLength);
				return BadRequest($"User name length must be more than {_appConfiguration.UserNameMinLength}");
			}

			User userInDb = await _bookingContext
				.Users
				.FirstOrDefaultAsync(u => u.UserName == user.UserName);

			if (userInDb != null)
			{
				_logger.LogWarning("Provided user already exists");
				return Conflict("User with this name already exists");
			}

			_bookingContext.Users.Add(user);
			await _bookingContext.SaveChangesAsync();

			return Ok(user);
		}
	}
}
