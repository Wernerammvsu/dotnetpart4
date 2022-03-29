namespace BookingPlatform.DataAccess.Models;

public class BookingDAL
{
	public int Id { get; set; }
	public string? Comment { get; set; }
	public DateTime FromUtc { get; set; }
	public DateTime ToUtc { get; set; }

	public string Username { get; set; } = null!;
	public UserDAL? User { get; set; }
	public int RoomId { get; set; }
	public RoomDAL? Room { get; set; }
}
