namespace BookingPlatform.Domain.Service
{
	public interface IBookingService
	{
		Task<IEnumerable<Entity.Booking>> FindBookingsByRoomIdAsync(int roomId);
		Task<IEnumerable<Entity.Booking>> FindBookingsWithDefaultRoomAsync();
		Task<Entity.Booking> CreateBookingAsync(Entity.Booking booking);
		Task<IEnumerable<Entity.Booking>> FindAllActiveAsync();
	}
}