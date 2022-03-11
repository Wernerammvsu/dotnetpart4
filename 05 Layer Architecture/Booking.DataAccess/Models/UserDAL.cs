namespace Booking.DataAccess.Models;

public class UserDAL
{
	public int Id { get; set; }
	public string UserName { get; set; } = null!;

	public ICollection<BookingDAL>? Bookings { get; set; }
}

