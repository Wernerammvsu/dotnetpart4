using Microsoft.AspNetCore.Mvc;
using MongoDbExample.DataAccess;
using MongoDbExample.DataAccess.Entity;
using MongoDbExample.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDbExample.Controllers
{
	[Route("booking")]
	public class BookingController : Controller
	{
		private readonly BookingRespository _bookingRespository;
		private readonly UserRepository _userRepository;

		public BookingController(BookingRespository bookingRespository,
			UserRepository userRepository)
		{
			_bookingRespository = bookingRespository;
			_userRepository = userRepository;
		}

		public async Task<IActionResult> GetAll()
		{
			var bookingsWithoutUsers = await _bookingRespository.GetAllAsync();
			IEnumerable<Task<BookingDto>> filledBookingsTask = bookingsWithoutUsers
				.Select(async booking =>
				{
					string username = (await _userRepository.FindUserByIdAsync(booking.UserId))?.UserName;
					return BookingDto.FromBooking(booking, username);
				});
			return Ok(await Task.WhenAll(filledBookingsTask));
		}

		[HttpPost]
		public async Task<ActionResult<BookingDto>> CreateBooking(BookingDto bookingDto)
		{
			if (string.IsNullOrEmpty(bookingDto.Username))
				return BadRequest("Username cannot be empty");
			if (bookingDto.FromUtc == default)
				return BadRequest("FromUtc cannot be empty");
			if (bookingDto.ToUtc == default)
				return BadRequest("ToUtc cannot be empty");

			User user = await _userRepository
				.FindUserAsync(bookingDto.Username);
			if (user is null)
				return BadRequest($"User with name '{bookingDto.Username}' cannot be found");

			Booking newBooking = bookingDto
				.ToBooking(userId: user.Id);
			var alreadyCreatedBookings = await _bookingRespository.FindAll(b =>
					b.FromUtc <= newBooking.FromUtc
					&& b.ToUtc >= newBooking.FromUtc
					|| b.FromUtc <= newBooking.ToUtc
					&& b.ToUtc >= newBooking.ToUtc
					|| b.FromUtc == newBooking.FromUtc
					&& b.ToUtc == newBooking.ToUtc);
			if (alreadyCreatedBookings.Any())
				return Conflict("Booking for this time has already been created");
			if (newBooking.FromUtc < DateTime.UtcNow)
				return BadRequest("Cannot have from date earlier than now");
			if (newBooking.ToUtc - newBooking.FromUtc <= TimeSpan.FromMinutes(30))
				return BadRequest("Booking period should be at lease 30 minutes long");

			await _bookingRespository.CreateAsync(newBooking);
			return Ok(BookingDto.FromBooking(newBooking, user.UserName));
		}
	}
}
