using EFCoreExample.DataAccess;
using EFCoreExample.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly BookingContext _bookingContext;

        public RoomController(BookingContext bookingContext)
        {
            _bookingContext = bookingContext;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> CreateRoom([FromBody] RoomDto roomDto)
        {
            var room = roomDto.ToRoom();

            var roomInDb = await _bookingContext
                .Rooms
                .FirstOrDefaultAsync(r => r.Name == room.Name);

            if (roomInDb != null)
            {
                return Conflict("Room already exists");
            }

            await _bookingContext.Rooms.AddAsync(room);

            return Ok(RoomDto.FromRoom(room));
        }

        public async Task<ActionResult<IEnumerable<RoomDto>>> FindEmptyRoom([FromQuery] DateTime fromUtc, DateTime toUtc)
        {
            var freeRooms = await _bookingContext
                .RoomBookings
                .Where(rb => rb.Booking.ToUtc <= fromUtc || rb.Booking.FromUtc >= toUtc)
                .Select(rb => RoomDto.FromRoom(rb.Room))
                .ToListAsync();

            return Ok(freeRooms);
        }
    }
}
