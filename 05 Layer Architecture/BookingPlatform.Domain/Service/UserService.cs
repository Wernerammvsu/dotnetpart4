using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Persistence;

namespace BookingPlatform.Domain.Service
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;

        public UserService(IPasswordHasher passwordHasher,
            IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(string username, string password)
        {
            User? userInDb = await _userRepository.FindByUsername(username);
            if (userInDb is not null)
                throw new Exception("Username is already taken");
            string passwordHash = _passwordHasher.HashPassword(password);
            return await _userRepository.CreateUserAsync(new User(username, passwordHash));
        }

        public async Task<User?> FindUserAsync(string username)
        {
            return await _userRepository.FindByUsername(username);
        }

        public async Task<User> GetByCredintialsAsync(string username, string password)
        {
            User? userInDb = await _userRepository.FindByUsername(username);
            if (userInDb is null)
                throw new Exception("Username is not found");
            bool passwordIsValid = _passwordHasher
                .VerifyHashedPassword(userInDb.PasswordHash, password);
            if (!passwordIsValid)
                throw new Exception("Credentials are not valid");
            return userInDb;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
    }
}
