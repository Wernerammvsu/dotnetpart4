namespace BookingPlatform.Domain.Service
{
	public interface IPasswordHasher
	{
		string HashPassword(string password);
		bool VerifyHashedPassword(string hashedPasswordString, string password);
	}
}