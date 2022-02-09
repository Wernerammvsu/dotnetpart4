using Microsoft.AspNetCore.Mvc;
using MongoDbExample.DataAccess;
using MongoDbExample.DataAccess.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbExample.Controllers
{
	[Route("user")]
	public class UserController : Controller
	{
		private readonly UserRepository _userRepository;

		public UserController(UserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<ActionResult<IEnumerable<string>>> GetAllUserNames()
		{
			return Ok((await _userRepository.FindAllAsync())
				.Select(u => u.UserName));
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
