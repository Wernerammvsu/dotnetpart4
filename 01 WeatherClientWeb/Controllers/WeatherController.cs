using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WeatherClientWeb.Controllers
{
	[Route("weather")]
	public class WeatherController : Controller
	{
		public ActionResult Get([Required, FromQuery(Name = "city")]string cityName)
		{
			if (!ModelState.IsValid)
				return BadRequest("Name of a city is not provided");
			return Ok("Succsdsd1sdsdsddfdsdsdsess");
		}
	}
}
