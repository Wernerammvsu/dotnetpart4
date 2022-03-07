using System.Collections.Generic;

namespace EFCoreExample.DataAccess.Entity
{
	public class Room
    {
		public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfBeds { get; set; }
        public int Mark { get; set; }

        public ICollection<RoomBooking> RoomBookings { get; set; }
    }
}
