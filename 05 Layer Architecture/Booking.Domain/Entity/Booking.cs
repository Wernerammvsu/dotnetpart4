namespace Booking.Domain.Entity;

public class Booking
{
	public const int DefaultRoomId = 1;

	public readonly int? Id;
	public readonly string? Comment;
	public readonly DateTime FromUtc;
	public readonly DateTime ToUtc;
	public readonly int UserId;
	public readonly int RoomId;

	public Booking(int? id, string? comment, DateTime fromUtc, DateTime toUtc, int userId, int roomId = DefaultRoomId)
	{
		Id = id;
		Comment = comment;
		FromUtc = fromUtc;
		ToUtc = toUtc;
		UserId = userId;
		RoomId = roomId;
	}
}
