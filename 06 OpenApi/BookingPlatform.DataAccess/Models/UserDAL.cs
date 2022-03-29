namespace BookingPlatform.DataAccess.Models;

public class UserDAL
{
	public string Username { get; set; } = null!;
	public string PasswordHash { get; set; } = null!;

	public ICollection<BookingDAL>? Bookings { get; set; }
}

