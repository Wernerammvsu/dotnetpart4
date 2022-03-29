using Microsoft.Extensions.Options;

namespace BookingPlatform.WebApi.Configuration
{
	public class AuthOptionsValidator : IValidateOptions<AuthOptions>
	{
		public ValidateOptionsResult Validate(string name, AuthOptions options)
		{
			if (string.IsNullOrEmpty(options.Issuer))
				return ValidateOptionsResult.Fail(nameof(options.Issuer));
			if (options.LifetimeInMinutes <= 0)
				return ValidateOptionsResult.Fail(nameof(options.LifetimeInMinutes));
			if (string.IsNullOrEmpty(options.SecretKey))
				return ValidateOptionsResult.Fail(nameof(options.SecretKey));
			return ValidateOptionsResult.Success;
		}
	}
}
