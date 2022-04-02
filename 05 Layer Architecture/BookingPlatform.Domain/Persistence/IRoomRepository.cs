using BookingPlatform.Domain.Entity;

namespace BookingPlatform.Domain.Persistence
{
    public interface IRoomRepository
    {
        Task<Room?> FindRoomByIdAsync(int roomId);
        Task<Room?> FindRoomByNameAsync(string roomName);
        Task<IEnumerable<Room>> GetAllRoomsWithBookingsAsync();
        Task<Room> CreateRoomAsync(Room room);
    }
}
