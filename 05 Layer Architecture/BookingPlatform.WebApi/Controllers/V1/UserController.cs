using BookingPlatform.DataAccess;
using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Service;
using BookingPlatform.WebApi.Configuration;
using BookingPlatform.WebApi.Dto.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.WebApi.Controllers.V1;

[ApiController]
[Route("v1/user")]
public class UserController : Controller
{
	private readonly BookingContext _bookingContext;
	private readonly IUserService _userService;
	private readonly AuthOptions _authOptions;

	public UserController(BookingContext bookingContext,
		IUserService userService,
		IOptions<AuthOptions> authOptions)
	{
		_bookingContext = bookingContext;
		_userService = userService;
		_authOptions = authOptions.Value;
	}

	public async Task<ActionResult<UserDAL[]>> GetAllUserNames()
	{
		return await _bookingContext.Users.ToArrayAsync();
	}

	[HttpPost]
	public async Task<ActionResult<User>> CreateUser([FromBody] UserDto user)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		return await _userService.CreateUserAsync(user.Username, user.Password);
	}

	[HttpPost]
	[Route("checkpassword")]
	public async Task<ActionResult<User>> CheckPassword([FromBody] UserDto user)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var userInDb = await _userService
			.GetByCredintialsAsync(user.Username, user.Password);

		return Ok(new { username = userInDb.Username });
	}

	[HttpPost]
	[Route("login")]
	public async Task<IActionResult> Authenticate(LoginUserDto loginUserDto)
	{
		var user = await _userService
			.GetByCredintialsAsync(loginUserDto.Username!, loginUserDto.Password!);

		// authentication successful so generate jwt token
		var secretKey = Encoding.ASCII.GetBytes(_authOptions.SecretKey);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, user.Username.ToString()),
			}),
			IssuedAt = DateTime.Now,
			Expires = DateTime.Now.AddMinutes(60),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
			Issuer = _authOptions.Issuer
		};
		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenDescriptor);
		var tokenString = tokenHandler.WriteToken(token);
		return Ok(tokenString);
	}
}
