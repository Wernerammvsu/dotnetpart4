using Microsoft.AspNetCore.Mvc;
using RedisCaching.Caching;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WeatherClientWeb.OpenWeather;

namespace WeatherClientWeb.Controllers
{
	[Route("weather")]
	public class WeatherController : Controller
	{
		private readonly RedisDictionary _redisDictionary;

		public WeatherController(RedisDictionary redisDictionary)
		{
			_redisDictionary = redisDictionary;
		}

		public async Task<ActionResult> Get([Required, FromQuery(Name = "city")]string cityName)
		{
			if (!ModelState.IsValid)
				return BadRequest("Name of a city is not provided");

			CurrentWeatherDto currentWeatherDto = await new OpenWeatherClient(_redisDictionary).GetWeatherAsync(cityName);

			return Ok(currentWeatherDto);
		}
	}
}
