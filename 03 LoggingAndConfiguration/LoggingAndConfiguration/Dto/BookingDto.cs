using EFCoreExample.DataAccess.Entity;
using System;

namespace EFCoreExample.Dto
{
	public class BookingDto
	{
		public string Username { get; set; }
		public DateTime FromUtc { get; set; }
		public DateTime ToUtc { get; set; }
		public string Comment { get; set; }
		public int RoomId { get; set; }

		public Booking ToBooking(int userId)
		{
			return new Booking
			{
				UserId = userId,
				FromUtc = DateTime.SpecifyKind(FromUtc, DateTimeKind.Utc),
				ToUtc = DateTime.SpecifyKind(ToUtc, DateTimeKind.Utc),
				Comment = Comment,
				RoomId = RoomId
			};
		}

		public static BookingDto FromBooking(Booking booking)
		{
			return new BookingDto
			{
				Comment = booking.Comment,
				FromUtc = booking.FromUtc,
				ToUtc = booking.ToUtc,
				Username = booking.User.UserName,
				RoomId = booking.RoomId
			};
		}
	}
}
