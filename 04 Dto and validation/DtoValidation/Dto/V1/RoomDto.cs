using DtoValidation.DataAccess.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DtoValidation.Dto.V1;

public class RoomDto 
{
    [Required]
	[MinLength(3, ErrorMessage = "Roomname must be at least three characters long")]
	[MaxLength(30, ErrorMessage = "Roomname must be no longer than thirty characters")]
    public string RoomName { get; set; } = null!;
    public IEnumerable<BookingDto>? BookingDtos { get; set; } 
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
			RoomName = room.RoomName,
            BookingDtos = room.Bookings.Select(b => BookingDto.FromBooking(b)).ToList()
		};
	}

}