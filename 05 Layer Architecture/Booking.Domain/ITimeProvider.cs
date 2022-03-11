namespace Booking.Domain
{
	public interface ITimeProvider
	{
		public DateTime UtcNow { get; }
	}
}
