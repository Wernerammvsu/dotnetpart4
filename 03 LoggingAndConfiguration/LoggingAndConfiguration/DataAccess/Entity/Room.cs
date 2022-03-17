using System.Collections.Generic;

namespace EFCoreExample.DataAccess.Entity
{
	public class Room
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public IEnumerable<Booking> Bookings { get; set; }

	}
}
