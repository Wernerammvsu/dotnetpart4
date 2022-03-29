using System;
using System.ComponentModel.DataAnnotations;

namespace BookingPlatform.WebApi.Dto.V1;

public class BookingDto
{
	/// <example>Host</example>
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

	public Domain.Entity.Booking ToBooking(string username)
	{
		return new Domain.Entity.Booking(id: null,
			comment: Comment,
			fromUtc: DateTime.SpecifyKind(FromUtc!.Value, DateTimeKind.Utc),
			toUtc: DateTime.SpecifyKind(ToUtc!.Value, DateTimeKind.Utc),
			username: username);
	}

	public static BookingDto FromBooking(Domain.Entity.Booking booking)
	{
		return new BookingDto
		{
			Comment = booking.Comment,
			FromUtc = booking.FromUtc,
			ToUtc = booking.ToUtc
		};
	}
}
