namespace BookingPlatform.Domain.Entity;

public class User
{
	public readonly string Username;
	public readonly string PasswordHash;

	public User(string username, string passwordHash)
	{
		Username = username;
		PasswordHash = passwordHash;
	}
}

