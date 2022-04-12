using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Persistence;

namespace BookingPlatform.Domain.Service
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Room?> FindRoomByIdAsync(int roomId)
        {
            return await _roomRepository.FindRoomByIdAsync(roomId);
        }

        public async Task<Room?> FindRoomByNameAsync(string roomName)
        {
            return await _roomRepository.FindRoomByNameAsync(roomName);
        }

        public async Task<IEnumerable<Room>> GetAllRoomsWithBookingsAsync()
        {
            return await _roomRepository.GetAllRoomsWithBookingsAsync();
        }

        public async Task<Room> CreateRoomAsync(Room room)
        {
            var roomInDatabase = await FindRoomByNameAsync(room.RoomName);

            if (roomInDatabase != null)
                throw new ArgumentException("Room with this name already exists");

            return await _roomRepository.CreateRoomAsync(room);
        }
    }
}
