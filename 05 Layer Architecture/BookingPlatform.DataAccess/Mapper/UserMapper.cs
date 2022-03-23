using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;

namespace BookingPlatform.DataAccess.Mapper
{
	public class UserMapper : IUserMapper
	{
		public User Map(UserDAL userDAL)
		{
			return new User(userDAL.Username, userDAL.PasswordHash);
		}

		public UserDAL Map(User user)
		{
			return new UserDAL { Username = user.Username, PasswordHash = user.PasswordHash };
		}
	}
}
