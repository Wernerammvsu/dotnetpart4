using System;
using System.Collections.Generic;

namespace EFCoreExample.DataAccess.Entity
{
	public class Booking
	{
		public int Id { get; set; }
		public string Comment { get; set; }
		public DateTime FromUtc { get; set; }
		public DateTime ToUtc { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }

		public ICollection<RoomBooking> RoomBookings { get; set; }
	}
}
