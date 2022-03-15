using DtoValidation.DataAccess;
using DtoValidation.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DtoValidation.Dto.V1;
using System.Linq;

namespace DtoValidation.Controllers.V2
{
	[ApiController]
	[Route("v2/room")]
	public class RoomController : Controller
	{
		private readonly BookingContext _bookingContext;

		public RoomController(BookingContext bookingContext)
		{
			_bookingContext = bookingContext;
		}

		public async Task<ActionResult<RoomDto[]>> GetAll()
		{
			var rooms = await _bookingContext.Rooms.Include(x => x.Bookings).ToArrayAsync();
			return rooms.Select(r => RoomDto.FromRoom(r)).ToArray();
		}

		[HttpPost]
		public async Task<ActionResult<RoomDto>> CreateRoom(RoomDto roomDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var room = roomDto.ToRoom();
			Room? roomInDatabase = await _bookingContext
				.Rooms
				.FirstOrDefaultAsync(u => u.RoomName == room.RoomName);

			if (roomInDatabase != null)
				return Conflict("User with this name already exists");

			_bookingContext.Rooms.Add(room);
			await _bookingContext.SaveChangesAsync();

			return Ok(RoomDto.FromRoom(room));
		}
	}
}
