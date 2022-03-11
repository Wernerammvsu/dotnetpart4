using DtoValidation.DataAccess.Entity;
using System;

namespace DtoValidation.Dto.V2;

public class BookingDto
{
	public string? Username { get; set; }
	public string? RoomName { get; set; }
	public DateTime? FromUtc { get; set; }
	public DateTime? ToUtc { get; set; }
	public string? Comment { get; set; }

	public Booking ToBooking(int userId, int roomId)
	{
		if (!FromUtc.HasValue)
			throw new ArgumentNullException("FromUtc");
		if (!ToUtc.HasValue)
			throw new ArgumentNullException("ToUtc");
		return new Booking(comment: Comment,
			fromUtc: DateTime.SpecifyKind(FromUtc.Value, DateTimeKind.Utc),
			toUtc: DateTime.SpecifyKind(ToUtc.Value, DateTimeKind.Utc),
			userId: userId,
			roomId: roomId);
	}

	public static BookingDto FromBooking(Booking booking)
	{
		var dto = new BookingDto();
		dto.Comment = booking.Comment;
		dto.FromUtc = booking.FromUtc;
		dto.ToUtc = booking.ToUtc;
		dto.Username = booking.User?.UserName;
		dto.RoomName = booking.Room?.RoomName;
		return dto;
	}
}
