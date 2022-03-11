using Booking.DataAccess;
using Booking.DataAccess.Models;
using Booking.Domain.Service;
using Booking.WebApi.Dto.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.WebApi.Controllers.V2;

[ApiController]
[Route("v2/booking")]
public class BookingController : Controller
{
	private readonly BookingContext _bookingContext;
	private readonly IBookingService _bookingService;

	public BookingController(BookingContext bookingContext,
		IBookingService bookingService)
	{
		_bookingContext = bookingContext;
		_bookingService = bookingService;
	}

	public async Task<IActionResult> GetAll()
	{
		//var bookings = await _bookingContext.Bookings
		//	.Include(b => b.User)
		//	.Include(b => b.Room)
		//	.ToArrayAsync();
		var bookings = await _bookingService.FindAllActiveAsync();
		return Ok(bookings.Select(b => BookingDto.FromBooking(b)));
	}

	[HttpPost]
	public async Task<ActionResult<BookingDto>> CreateBooking(BookingDto bookingDto)
	{
		// Validate Dto
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		// Find User
		UserDAL? user = await _bookingContext
			.Users
			.FirstOrDefaultAsync(u => u.UserName == bookingDto.Username);
		if (user is null)
			return BadRequest($"User with name '{bookingDto.Username}' cannot be found");

		// Find Room
		RoomDAL? room = await _bookingContext
			.Rooms
			.FirstOrDefaultAsync(r => r.RoomName == bookingDto.RoomName);
		if (room is null)
			return BadRequest($"Room with name '{bookingDto.RoomName}' cannot be found");

		Domain.Entity.Booking newBooking = bookingDto.ToBooking(user.Id, room.Id);
		newBooking = await _bookingService.CreateBookingAsync(newBooking);
		return Ok(BookingDto.FromBooking(newBooking));
	}
}
