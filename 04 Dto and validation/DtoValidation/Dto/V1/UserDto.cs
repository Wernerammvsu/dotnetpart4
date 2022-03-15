using DtoValidation.DataAccess.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DtoValidation.Dto.V1;

public class UserDto
{
    [Required]
	[MinLength(3, ErrorMessage = "Username must be at least three characters long")]
	[MaxLength(30, ErrorMessage = "Username must be no longer than thirty characters")]
    public string UserName { get; set; } = null!;
    public IEnumerable<BookingDto>? BookingDtos { get; set; } 

    public User ToUser()
	{
		return new User
        {
            UserName = UserName
        };
	}

	public static UserDto FromUser(User user)
	{
		return new UserDto
		{
			UserName = user.UserName,
            BookingDtos = user.Bookings.Select(b => BookingDto.FromBooking(b)).ToList()
		};
	}
}