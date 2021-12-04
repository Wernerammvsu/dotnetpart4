using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherClientWeb.OpenWeather
{
	public class OpenWeatherClient
	{
		const string defaultLanguage = "ru";
		const string apiKey = "7b6be55ecfc023f52792505653e8e278";
		const string urlTemplate = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&lang={2}";

		// Will be used later
		// const string iconUrlTemplate = "http://openweathermap.org/img/w/{0}.png";

		public async ValueTask<CurrentWeatherDto> GetWeatherAsync(string cityName)
		{
			var lowerCasedCityName = cityName.ToLower();

			string currentWeatherUrl = string.Format(urlTemplate, lowerCasedCityName, apiKey, defaultLanguage);
			var httpClient = new HttpClient();

			var response = await httpClient.GetAsync(currentWeatherUrl);
			if (!response.IsSuccessStatusCode)
				throw new Exception($"Openweathermap response has a fault code {response.StatusCode}");
			var currentWeatherJson = await response.Content.ReadAsStringAsync();

			JsonDocument currentWeatherDocument = JsonDocument.Parse(currentWeatherJson);
			return currentWeatherDocument.Deserialize<CurrentWeatherDto>();
		}
	}
}
