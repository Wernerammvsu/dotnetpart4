namespace Booking.Domain.Persistence
{
	public interface IBookingRepository
	{
		Task<IEnumerable<Entity.Booking>> FindBookingsByRoomIdAsync(int roomId);
		Task<Entity.Booking?> FindActiveBookingInRoomAsync(int roomId, DateTime fromUtc, DateTime toUtc);
		Task<Entity.Booking> SaveAsync(Entity.Booking booking);
		Task<IEnumerable<Entity.Booking>> FindWithEndEarlierThanAsync(DateTime dateTimeUtc);
	}
}
