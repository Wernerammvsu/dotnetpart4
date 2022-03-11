using System;

namespace DtoValidation.DataAccess.Entity;

public class Booking
{
	public const int DefaultRoomId = 1;

	public int Id { get; set; }
	public string? Comment { get; set; }
	public DateTime FromUtc { get; set; }
	public DateTime ToUtc { get; set; }

	public int UserId { get; set; }
	public User? User { get; set; }
	public int RoomId { get; set; }
	public Room? Room { get; set; }

	public Booking() { }

	public Booking(string? comment, DateTime fromUtc, DateTime toUtc, int userId, int roomId = DefaultRoomId)
	{
		Comment = comment;
		FromUtc = fromUtc;
		ToUtc = toUtc;
		UserId = userId;
		RoomId = roomId;
	}
}
