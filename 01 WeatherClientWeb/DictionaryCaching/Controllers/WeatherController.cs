using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WeatherClientWeb.OpenWeather;

namespace WeatherClientWeb.Controllers
{
	[Route("weather")]
	public class WeatherController : Controller
	{
		private readonly OpenWeatherClient _openWeatherClient;

		public WeatherController(OpenWeatherClient openWeatherClient)
		{
			_openWeatherClient = openWeatherClient;
		}

		public async Task<ActionResult> Get([Required, FromQuery(Name = "city")]string cityName)
		{
			if (!ModelState.IsValid)
				return BadRequest("Name of a city is not provided");

			CurrentWeatherDto currentWeatherDto = await _openWeatherClient.GetWeatherAsync(cityName);

			return Ok(currentWeatherDto);
		}
	}
}
