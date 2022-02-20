using EFCoreExample.DataAccess.Entity;

namespace EFCoreExample.Dto
{
    public class RoomDto
    {
        public int NumberOfBeds { get; set; }
        public int Mark { get; set; }
		public string Name { get; set; }

		public Room ToRoom()
		{
			return new Room
			{
				NumberOfBeds = this.NumberOfBeds,
				Mark = this.Mark,
				Name = this.Name
			};
		}

		public static RoomDto FromRoom(Room room)
		{
			return new RoomDto
			{
				NumberOfBeds = room.NumberOfBeds,
				Mark = room.Mark,
				Name = room.Name
			};
		}

	}
}
