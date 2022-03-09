using Booking.DataAccess;
using Booking.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Booking.WebApi.Controllers.V2
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

		public async Task<ActionResult<RoomDAL[]>> GetAll()
		{
			return await _bookingContext.Rooms.Include(x => x.Bookings).ToArrayAsync();
		}

		[HttpPost]
		public async Task<ActionResult<RoomDAL>> CreateRoom(RoomDAL room)
		{
			RoomDAL? roomInDatabase = await _bookingContext
				.Rooms
				.FirstOrDefaultAsync(u => u.RoomName == room.RoomName);

			if (roomInDatabase != null)
				return Conflict("User with this name already exists");

			_bookingContext.Rooms.Add(room);
			await _bookingContext.SaveChangesAsync();

			return Ok(room);
		}
	}
}
