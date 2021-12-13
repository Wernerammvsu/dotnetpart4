using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WeatherClientWeb.Dto;
using WeatherClientWeb.OpenWeather;

namespace WeatherClientWeb.Controllers
{
	[Route("weather")]
	public class WeatherController : Controller
	{
		public async Task<ActionResult> Get([Required, FromQuery(Name = "city")]string cityName)
		{
			if (!ModelState.IsValid)
				return BadRequest("Name of a city is not provided");

			var openWeatherClient = new OpenWeather.OpenWeatherClient();

			CurrentWeatherDto currentWeatherDto = await openWeatherClient.GetWeatherAsync(cityName);

			return Ok(currentWeatherDto);
		}

		[Route("user/{id}")]
		public Task<ActionResult> AuthorGet([FromRoute]UserDto userDto, [FromQuery]int tid)
		{
			return Task.FromResult((ActionResult)Ok(userDto));
		}
	}
}
