using BookingPlatform.Domain.Entity;

namespace BookingPlatform.Domain.Service
{
    public interface IRoomService
    {
        Task<Room> FindRoomByIdAsync(int roomId);
        Task<Room> FindRoomByNameAsync(string roomName);
        Task<IEnumerable<Room>> GetAllRoomsWithBookingsAsync();
        Task<Room> CreateRoomAsync(Room room);
    }
}
