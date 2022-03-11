using System;

namespace Booking.WebApi.Dto.V2;

public class BookingDto
{
	public string? Username { get; set; }
	public string? RoomName { get; set; }
	public DateTime? FromUtc { get; set; }
	public DateTime? ToUtc { get; set; }
	public string? Comment { get; set; }

	public Domain.Entity.Booking ToBooking(int userId, int roomId)
	{
		if (!FromUtc.HasValue)
			throw new ArgumentNullException("FromUtc");
		if (!ToUtc.HasValue)
			throw new ArgumentNullException("ToUtc");
		return new Domain.Entity.Booking(id: null,
			comment: Comment,
			fromUtc: DateTime.SpecifyKind(FromUtc.Value, DateTimeKind.Utc),
			toUtc: DateTime.SpecifyKind(ToUtc.Value, DateTimeKind.Utc),
			userId: userId,
			roomId: roomId);
	}

	public static BookingDto FromBooking(Domain.Entity.Booking booking)
	{
		var dto = new BookingDto();
		dto.Comment = booking.Comment;
		dto.FromUtc = booking.FromUtc;
		dto.ToUtc = booking.ToUtc;
		return dto;
	}
}
