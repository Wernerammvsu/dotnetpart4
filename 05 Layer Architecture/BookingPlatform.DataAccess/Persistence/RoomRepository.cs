using BookingPlatform.DataAccess.Mapper;
using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.DataAccess.Persistence
{
    public class RoomRepository : IRoomRepository
    {
        private readonly BookingContext _bookingContext;
        private readonly IRoomMapper _roomMapper;

        public RoomRepository(BookingContext bookingContext,
            IRoomMapper roomMapper)
        {
            _bookingContext = bookingContext;
            _roomMapper = roomMapper;
        }

        public async Task<Room?> FindRoomByIdAsync(int roomId)
        {
            var room = await _bookingContext
                .Rooms
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room is null)
            {
                return null;
            }

            return _roomMapper.Map(room);
        }

        public async Task<Room?> FindRoomByNameAsync(string roomName)
        {
            var room = await _bookingContext
                .Rooms
                .FirstOrDefaultAsync(u => u.RoomName == roomName);

            if (room is null)
            {
                return null;
            }

            return _roomMapper.Map(room);
        }

        public async Task<IEnumerable<Room>> GetAllRoomsWithBookingsAsync()
        {
            var rooms = await _bookingContext.Rooms.Include(x => x.Bookings).ToArrayAsync();

            return rooms.Select(room => _roomMapper.Map(room));
        }

        public async Task<Room> CreateRoomAsync(Room room)
        {
            var roomDAL = _roomMapper.Map(room);
            await _bookingContext.Rooms.AddAsync(roomDAL);
            await _bookingContext.SaveChangesAsync();

            return _roomMapper.Map(roomDAL);
        }
    }
}
