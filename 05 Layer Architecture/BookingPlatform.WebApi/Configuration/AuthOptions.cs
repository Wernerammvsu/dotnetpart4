namespace BookingPlatform.WebApi.Configuration
{
	public class AuthOptions
	{
		public string Issuer { get; set; } = null!;
		public string SecretKey { get; set; } = null!;
		public int LifetimeInMinutes { get; set; }
	}
}
