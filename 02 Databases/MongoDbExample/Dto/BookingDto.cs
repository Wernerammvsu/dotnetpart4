using MongoDbExample.DataAccess.Entity;
using System;

namespace MongoDbExample.Dto
{
	public class BookingDto
	{
		public string Username { get; set; }
		public DateTime FromUtc { get; set; }
		public DateTime ToUtc { get; set; }
		public string Comment { get; set; }

		public Booking ToBooking(string userId)
		{
			return new Booking
			{
				UserId = userId,
				FromUtc = DateTime.SpecifyKind(FromUtc, DateTimeKind.Utc),
				ToUtc = DateTime.SpecifyKind(ToUtc, DateTimeKind.Utc),
				Comment = Comment
			};
		}

		public static BookingDto FromBooking(Booking booking, string username)
		{
			return new BookingDto
			{
				Comment = booking.Comment,
				FromUtc = booking.FromUtc,
				ToUtc = booking.ToUtc,
				Username = username
			};
		}
	}
}
