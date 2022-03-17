using EFCoreExample.DataAccess;
using EFCoreExample.DataAccess.Entity;
using EFCoreExample.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDbExample.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EFCoreExample.Controllers
{
	[Route("booking")]
	public class BookingController : Controller
	{
		private readonly BookingContext _bookingContext;
		private readonly AppConfiguration _appConfiguration;
		private readonly ILogger<BookingController> _logger;


		public BookingController(BookingContext bookingContext,
			IOptions<AppConfiguration> options, 
			ILogger<BookingController> logger)
		{
			_bookingContext = bookingContext;
			_appConfiguration = options.Value;
			_logger = logger;
		}
		
		[Route("{id}")]
		public IActionResult Get(int id)
		{
			return Ok(id);
		}

		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var bookings = await _bookingContext.Bookings
				.Include(b => b.User)
				.ToArrayAsync(cancellationToken);
			return Ok(bookings.Select(b => BookingDto.FromBooking(b)));
		}

		[HttpPost]
		public async Task<ActionResult<BookingDto>> CreateBooking(BookingDto bookingDto)
		{
			if (string.IsNullOrEmpty(bookingDto.Username))
			{				 
				_logger.LogWarning("Provided username is empty");
  				return BadRequest("Username cannot be empty");
			}
			if (bookingDto.FromUtc == default)
			{
				_logger.LogWarning("Provided FromUtc is empty");
				return BadRequest("FromUtc cannot be empty");
			}
			if (bookingDto.ToUtc == default)
			{
				_logger.LogWarning("Provided ToUtc is empty");
				return BadRequest("ToUtc cannot be empty");
			}

			User user = await _bookingContext
				.Users
				.FirstOrDefaultAsync(u => u.UserName == bookingDto.Username);
			if (user is null)
			{
				_logger.LogWarning("Provided username = {Username} is not found", bookingDto.Username);
				return BadRequest($"User with name '{bookingDto.Username}' cannot be found");
			}

			Booking newBooking = bookingDto
				.ToBooking(userId: user.Id);
			var alreadyCreatedBooking = await _bookingContext
				.Bookings
				.FirstOrDefaultAsync(b =>
					b.FromUtc <= newBooking.FromUtc
					&& b.ToUtc >= newBooking.FromUtc
					|| b.FromUtc <= newBooking.ToUtc
					&& b.ToUtc >= newBooking.ToUtc
					|| b.FromUtc == newBooking.FromUtc
					&& b.ToUtc == newBooking.ToUtc);
			if (alreadyCreatedBooking is not null)
			{	
				_logger.LogWarning("Provided booking for this time has already been created");
				return Conflict("Booking for this time has already been created");
			}
			if (newBooking.FromUtc < DateTime.UtcNow)
			{
				_logger.LogWarning("Provided from date is earlier than now");
				return BadRequest("Cannot have from date earlier than now");
			}
			if (newBooking.ToUtc - newBooking.FromUtc <= TimeSpan.FromMinutes(_appConfiguration.MinTimeSpanForBookingInMinutes))
			{
				_logger.LogWarning("Provided booking period is less than {MinTimeSpanForBookingInMinutes} minutes", _appConfiguration.MinTimeSpanForBookingInMinutes);
				return BadRequest($"Booking period should be at least {_appConfiguration.MinTimeSpanForBookingInMinutes} minutes long");
			}

			_bookingContext.Add(newBooking);
			await _bookingContext.SaveChangesAsync();
			return Ok(BookingDto.FromBooking(newBooking));
		}
	}
}
