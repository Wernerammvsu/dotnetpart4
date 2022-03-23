using BookingPlatform.DataAccess;
using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Service;
using BookingPlatform.Domain.Service.DomainException;
using BookingPlatform.WebApi.Dto.V2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookingPlatform.WebApi.Controllers.V2;

[Authorize]
[Route("v2/booking")]
public class BookingController : Controller
{
	private readonly BookingContext _bookingContext;
	private readonly IBookingService _bookingService;
	private readonly IUserService _userService;

	public BookingController(BookingContext bookingContext,
		IBookingService bookingService,
		IUserService userService)
	{
		_bookingContext = bookingContext;
		_bookingService = bookingService;
		_userService = userService;
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
	public async Task<ActionResult<BookingDto>> CreateBooking([FromBody]BookingDto bookingDto)
	{
		// Validate Dto
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		string username = User.Identity!.Name!;

		// Find Room
		RoomDAL? room = await _bookingContext
			.Rooms
			.FirstOrDefaultAsync(r => r.Id == bookingDto.RoomId!.Value);
		if (room is null)
			throw new RoomNotFoundException(bookingDto.RoomId!.Value);

		Booking newBooking = bookingDto.ToBooking(username);
		newBooking = await _bookingService.CreateBookingAsync(newBooking);
		return Ok(BookingDto.FromBooking(newBooking));
	}
}
