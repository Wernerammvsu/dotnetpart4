using DtoValidation.DataAccess;
using DtoValidation.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using DtoValidation.Dto.V1;

namespace DtoValidation.Controllers.V1;

[Route("v1/user")]
public class UserController : Controller
{
	private readonly BookingContext _bookingContext;

	public UserController(BookingContext bookingContext)
	{
		_bookingContext = bookingContext;
	}

	public async Task<ActionResult<UserDto[]>> GetAllUserNames()
	{
		var users = await _bookingContext.Users.ToArrayAsync();
		return users.Select(u => UserDto.FromUser(u)).ToArray();
	}

	[HttpPost]
	public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto userDto)
	{	
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
		var user = userDto.ToUser();
		User? userInDb = await _bookingContext
			.Users
			.FirstOrDefaultAsync(u => u.UserName == user.UserName);

		if (userInDb != null)
			return Conflict("User with this name already exists");

		_bookingContext.Users.Add(user);
		await _bookingContext.SaveChangesAsync();

		return Ok(UserDto.FromUser(user));
	}
}
