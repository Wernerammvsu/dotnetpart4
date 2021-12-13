using Microsoft.AspNetCore.Mvc;
using WeatherClientWeb.Binding;

namespace WeatherClientWeb.Dto
{
	[ModelBinder(BinderType = typeof(UserDtoBinder))]
	public class UserDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
