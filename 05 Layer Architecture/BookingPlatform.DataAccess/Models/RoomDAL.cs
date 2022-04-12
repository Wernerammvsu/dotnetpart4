using System.Collections.ObjectModel;

namespace BookingPlatform.DataAccess.Models
{
	public class RoomDAL
	{
		public int Id { get; set; }
		public string RoomName { get; set; } = null!;
		public ICollection<BookingDAL> Bookings { get; set; }

		public RoomDAL()
		{
			Bookings = new Collection<BookingDAL>();
		}
	}
}
