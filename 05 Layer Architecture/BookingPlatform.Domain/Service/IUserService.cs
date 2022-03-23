using BookingPlatform.Domain.Entity;

namespace BookingPlatform.Domain.Service
{
	public interface IUserService
	{
		Task<User> CreateUserAsync(string username, string password);
		Task<User> GetByCredintialsAsync(string username, string password);
		Task<User?> FindUserAsync(string username);
	}
}