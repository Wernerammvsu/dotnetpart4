using BookingPlatform.Domain.Entity;

namespace BookingPlatform.Domain.Persistence
{
	public interface IBookingRepository
	{
		Task<IEnumerable<Booking>> FindBookingsByRoomIdAsync(int roomId);
		Task<Booking?> FindActiveBookingInRoomAsync(int roomId, DateTime fromUtc, DateTime toUtc);
		Task<Booking> SaveAsync(Entity.Booking booking);
		Task<IEnumerable<Booking>> FindWithEndEarlierThanAsync(DateTime dateTimeUtc);
	}
}
