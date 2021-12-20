using EFCoreExample.DataAccess;
using EFCoreExample.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EFCoreExample.Controllers
{
	[Route("user")]
	public class UserController : Controller
	{
		private readonly BookingContext _bookingContext;

		public UserController(BookingContext bookingContext)
		{
			_bookingContext = bookingContext;
		}

		[HttpPost]
		public async Task<ActionResult<User>> CreateUser([FromBody]User user)
		{
			User userInDb = await _bookingContext.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

			if (userInDb != null)
				return Conflict("User with this name already exists");

			_bookingContext.Users.Add(user);
			await _bookingContext.SaveChangesAsync();

			await _bookingContext.Bookings.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();

			return Ok(user);
		}
	}
}
