using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Service;
using BookingPlatform.WebApi.Configuration;
using BookingPlatform.WebApi.Dto.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookingPlatform.WebApi.Controllers.V1;

[ApiController]
[Route("v1/user")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly AuthOptions _authOptions;

    public UserController(IUserService userService,
        IOptions<AuthOptions> authOptions)
    {
        _userService = userService;
        _authOptions = authOptions.Value;
    }

    public async Task<ActionResult<User[]>> GetAllUserNames()
    {
        var users = await _userService.GetAllUsersAsync();

        return users.ToArray();
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
                new Claim(ClaimTypes.Name, user.Username.ToString())
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha512),
            #region Dont look
            Issuer = _authOptions.Issuer,
            IssuedAt = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(60)
            #endregion
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return Ok(tokenString);
    }
}
