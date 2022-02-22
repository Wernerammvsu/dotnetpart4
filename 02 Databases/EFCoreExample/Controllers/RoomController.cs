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

        public async Task<ActionResult<IEnumerable<RoomDto>>> FindEmptyRoom(DateTime fromUtc, DateTime toUtc)
        {
            var roomBookings = await _bookingContext
                .RoomBookings
                .Where(rb => rb.Booking.ToUtc <= fromUtc || rb.Booking.FromUtc >= toUtc)
                .ToListAsync();

            var freeRooms = roomBookings
                .Select(rb => RoomDto.FromRoom(rb.Room))
                .ToList();

            var roomWithBookingsIds = await _bookingContext
                .RoomBookings
                .Select(rb => rb.RoomId)
                .ToListAsync();

            var roomsWithNoBookings = await _bookingContext
                .Rooms
                .Where(r => !roomWithBookingsIds.Contains(r.Id))
                .Select(r => RoomDto.FromRoom(r))
                .ToListAsync();

            freeRooms.AddRange(roomsWithNoBookings);

            return Ok(freeRooms);
        }
    }
}
