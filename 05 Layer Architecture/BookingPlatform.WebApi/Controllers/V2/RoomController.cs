using System.Linq;
using System.Threading.Tasks;
using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Service;
using BookingPlatform.WebApi.Dto.V1;
using Microsoft.AspNetCore.Mvc;

namespace BookingPlatform.WebApi.Controllers.V2
{
    [ApiController]
    [Route("v2/room")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task<ActionResult<Room[]>> GetAll()
        {
            var rooms = await _roomService.GetAllRoomsWithBookingsAsync();

            return rooms.ToArray();
        }

        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom([FromBody] RoomDto room)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await _roomService.CreateRoomAsync(room.ToRoom());
        }
    }
}
