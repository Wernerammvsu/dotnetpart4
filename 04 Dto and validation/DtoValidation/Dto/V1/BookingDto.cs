using DtoValidation.DataAccess.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DtoValidation.Dto.V1;

public class BookingDto
{
	[Required]
	[MinLength(3, ErrorMessage = "Username must be at least three characters long")]
	[MaxLength(15, ErrorMessage = "Username must be no longer than fifteen characters")]
	public string? Username { get; set; } = null!;
	[Required]
	public DateTime? FromUtc { get; set; }
	[Required]
	public DateTime? ToUtc { get; set; }
	[MinLength(10, ErrorMessage = "Comment must be at least three ten characters long")]
	public string? Comment { get; set; }

	public Booking ToBooking(int userId)
	{
		return new Booking(comment: Comment,
			fromUtc: DateTime.SpecifyKind(FromUtc!.Value, DateTimeKind.Utc),
			toUtc: DateTime.SpecifyKind(ToUtc!.Value, DateTimeKind.Utc),
			userId: userId);
	}

	public static BookingDto FromBooking(Booking booking)
	{
		return new BookingDto
		{
			Comment = booking.Comment,
			FromUtc = booking.FromUtc,
			ToUtc = booking.ToUtc,
			Username = booking.User?.UserName
		};
	}
}
