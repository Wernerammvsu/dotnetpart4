using Booking.DataAccess;
using Booking.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Booking.WebApi.Controllers.V1;

[Route("v1/user")]
public class UserController : Controller
{
	private readonly BookingContext _bookingContext;

	public UserController(BookingContext bookingContext)
	{
		_bookingContext = bookingContext;
	}

	public async Task<ActionResult<UserDAL[]>> GetAllUserNames()
	{
		return await _bookingContext.Users.ToArrayAsync();
	}

	[HttpPost]
	public async Task<ActionResult<UserDAL>> CreateUser([FromBody] UserDAL user)
	{
		UserDAL? userInDb = await _bookingContext
			.Users
			.FirstOrDefaultAsync(u => u.UserName == user.UserName);

		if (userInDb != null)
			return Conflict("User with this name already exists");

		_bookingContext.Users.Add(user);
		await _bookingContext.SaveChangesAsync();

		return Ok(user);
	}
}
