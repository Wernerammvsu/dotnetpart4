using BookingPlatform.DataAccess.Mapper;
using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.DataAccess.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly BookingContext _bookingContext;
        private readonly IUserMapper _userMapper;

        public UserRepository(BookingContext bookingContext,
            IUserMapper userMapper)
        {
            _bookingContext = bookingContext;
            _userMapper = userMapper;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var dal = _userMapper.Map(user);
            _bookingContext.Users.Add(dal);
            await _bookingContext.SaveChangesAsync();
            return _userMapper.Map(dal);
        }

        public async Task<User?> FindByUsername(string username)
        {
            if (username is null)
            {
                return null;
            }

            var loweredUsername = username.ToLower();
            UserDAL? user = await _bookingContext.Users
                .Where(u => u.Username.ToLower() == loweredUsername)
                .FirstOrDefaultAsync();
            if (user is null)
                return null;
            return _userMapper.Map(user);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _bookingContext.Users.ToArrayAsync();

            return users.Select(user => _userMapper.Map(user));
        }
    }
}
