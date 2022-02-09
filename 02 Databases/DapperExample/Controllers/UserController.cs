using DapperExample.DataAccess;
using DapperExample.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DapperExample.Controllers
{
	[Route("user")]
	public class UserController : Controller
	{
		private readonly UserRepository _userRepository;

		public UserController(UserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[HttpPost]
		public async Task<ActionResult<User>> CreateUser([FromBody] User user)
		{
			User userInDb = await _userRepository.FindUserAsync(user.UserName);
			if (userInDb is not null)
				return Conflict("User with this name already exists");

			await _userRepository.CreateUserAsync(user);

			return Ok(user);
		}
	}
}
