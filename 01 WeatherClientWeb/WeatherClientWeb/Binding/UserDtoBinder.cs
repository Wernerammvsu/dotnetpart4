using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using WeatherClientWeb.Dto;

namespace WeatherClientWeb.Binding
{
	public class UserDtoBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			var modelName = bindingContext.ModelName;

			// Try to fetch the value of the argument by name
			var providedId = bindingContext.ValueProvider.GetValue("id");
			var providedTid = bindingContext.ValueProvider.GetValue("tid");

			if (providedId == ValueProviderResult.None)
			{
				return Task.CompletedTask;
			}

			if (!int.TryParse(providedId.FirstValue, out int userId))
			{
				// Non-integer arguments result in model state errors
				bindingContext.ModelState.TryAddModelError(
					modelName, "User Id must be an integer.");

				return Task.CompletedTask;
			}

			var model = new UserDto { Id = userId, Name = "Test user" };
			bindingContext.Result = ModelBindingResult.Success(model);
			return Task.CompletedTask;
		}
	}
}
