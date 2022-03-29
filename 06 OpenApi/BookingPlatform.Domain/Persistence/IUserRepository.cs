using BookingPlatform.Domain.Entity;

namespace BookingPlatform.Domain.Persistence
{
	public interface IUserRepository
	{
		Task<User?> FindByUsername(string username);
		Task<User> CreateUserAsync(User user);
	}
}
