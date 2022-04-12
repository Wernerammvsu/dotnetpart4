using System.ComponentModel.DataAnnotations;
using BookingPlatform.Domain.Entity;

namespace BookingPlatform.WebApi.Dto.V1
{
    public class RoomDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "RoomName must be at least three characters long")]
        [MaxLength(15, ErrorMessage = "RoomName must be no longer than fifteen characters")]
        public string RoomName { get; set; } = null!;

        public Room ToRoom()
        {
            return new Room
            {
                RoomName = RoomName
            };
        }

        public static RoomDto FromRoom(Room room)
        {
            return new RoomDto
            {
                RoomName = room.RoomName
            };
        }
    }
}
