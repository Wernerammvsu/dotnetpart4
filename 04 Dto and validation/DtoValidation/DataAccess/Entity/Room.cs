using System.Collections.Generic;

namespace DtoValidation.DataAccess.Entity
{
	public class Room
	{
		public int Id { get; set; }
		public string RoomName { get; set; } = null!;

		public ICollection<Booking>? Bookings { get; set; }
	}
}
