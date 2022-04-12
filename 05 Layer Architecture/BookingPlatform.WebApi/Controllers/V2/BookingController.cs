using System.Linq;
using System.Threading.Tasks;
using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Service;
using BookingPlatform.Domain.Service.DomainException;
using BookingPlatform.WebApi.Dto.V2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.WebApi.Controllers.V2;

[Authorize]
[Route("v2/booking")]
public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;
    private readonly IRoomService _roomService;

    public BookingController(IBookingService bookingService,
        IUserService userService,
        IRoomService roomService)
    {
        _bookingService = bookingService;
        _userService = userService;
        _roomService = roomService;
    }

    public async Task<IActionResult> GetAll()
    {
        var bookings = await _bookingService.FindAllActiveAsync();

        return Ok(bookings.Select(b => BookingDto.FromBooking(b)));
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] BookingDto bookingDto)
    {
        // Validate Dto
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string username = User.Identity!.Name!;

        // Find Room
        var room = await _roomService.FindRoomByIdAsync(bookingDto.RoomId!.Value);
        if (room is null)
            throw new RoomNotFoundException(bookingDto.RoomId!.Value);

        Booking newBooking = bookingDto.ToBooking(username);
        newBooking = await _bookingService.CreateBookingAsync(newBooking);
        return Ok(BookingDto.FromBooking(newBooking));
    }
}
