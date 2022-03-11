using AutoMapper;
using DtoValidation.DataAccess;
using DtoValidation.DataAccess.Entity;
using DtoValidation.Dto.V2;
using DtoValidation.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DtoValidation.Controllers.V2;

[ApiController]
[Route("v2/booking")]
public class BookingController : Controller
{
	private readonly BookingContext _bookingContext;
	private readonly BookingService _bookingService;
	private readonly IMapper _mapper;

	public BookingController(BookingContext bookingContext,
		BookingService bookingService,
		IMapper mapper)
	{
		_bookingContext = bookingContext;
		_bookingService = bookingService;
		_mapper = mapper;
	}

	public async Task<IActionResult> GetAll()
	{
		//var bookings = await _bookingContext.Bookings
		//	.Include(b => b.User)
		//	.Include(b => b.Room)
		//	.ToArrayAsync();
		var bookings1 = await _bookingContext.Bookings
			.Include(b => b.User)
			.Include(b => b.Room)
			.Select(b => BookingDto.FromBooking(b))
			.ToListAsync();
		return Ok(bookings1);
	}

	[HttpPost]
	public async Task<ActionResult<BookingDto>> CreateBooking(BookingDto bookingDto)
	{
		// Validate Dto
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		// Find User
		User? user = await _bookingContext
			.Users
			.FirstOrDefaultAsync(u => u.UserName == bookingDto.Username);
		if (user is null)
			return BadRequest($"User with name '{bookingDto.Username}' cannot be found");

		// Find Room
		Room? room = await _bookingContext
			.Rooms
			.FirstOrDefaultAsync(r => r.RoomName == bookingDto.RoomName);
		if (room is null)
			return BadRequest($"Room with name '{bookingDto.RoomName}' cannot be found");

		Booking newBooking = bookingDto.ToBooking(user.Id, room.Id);
		newBooking = await _bookingService.CreateBookingAsync(newBooking);
		return Ok(BookingDto.FromBooking(newBooking));
	}
}
