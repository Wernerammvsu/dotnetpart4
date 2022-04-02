using BookingPlatform.Domain.Entity;

namespace BookingPlatform.Domain.Service
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> FindBookingsByRoomIdAsync(int roomId);
        Task<IEnumerable<Booking>> FindBookingsWithDefaultRoomAsync();
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> FindAllActiveAsync();
    }
}