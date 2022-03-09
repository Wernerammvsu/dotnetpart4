namespace Booking.Domain.Entity;

public class User
{
	public int Id { get; set; }
	public string UserName { get; set; } = null!;

	public ICollection<Booking>? Bookings { get; set; }
}

