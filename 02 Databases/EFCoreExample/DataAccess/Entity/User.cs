using System.Collections.Generic;

namespace EFCoreExample.DataAccess.Entity
{
	public class User
	{
		public int Id { get; set; }
		public string UserName { get; set; }

		public ICollection<Booking> Bookings { get; set; }
	}
}

