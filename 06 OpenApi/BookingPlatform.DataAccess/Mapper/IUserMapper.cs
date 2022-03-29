using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;

namespace BookingPlatform.DataAccess.Mapper
{
	public interface IUserMapper
	{
		UserDAL Map(User user);
		User Map(UserDAL userDAL);
	}
}