namespace BookingPlatform.Domain
{
	public interface ITimeProvider
	{
		public DateTime UtcNow { get; }
	}
}
