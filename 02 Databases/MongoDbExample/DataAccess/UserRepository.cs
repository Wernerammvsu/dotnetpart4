using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDbExample.Configuration;
using MongoDbExample.DataAccess.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDbExample.DataAccess
{
	public class UserRepository
	{
		private readonly IMongoCollection<User> _usersCollection;

		public UserRepository(BookingDatabaseConfiguration bookingDatabaseConfiguration)
		{
			var mongoClient = new MongoClient(bookingDatabaseConfiguration.ConnectionString);

			IMongoDatabase mongoDatabase =
				mongoClient.GetDatabase(bookingDatabaseConfiguration.DatabaseName);

			_usersCollection = mongoDatabase.GetCollection<User>("Users");
		}

		public async Task<IEnumerable<User>> FindAllAsync()
		{
			return await _usersCollection.Find(_ => true).ToListAsync();
		}

		public async Task<User> FindUserByIdAsync(string userId)
		{
			return await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();
		}

		public async Task<User> FindUserAsync(string username)
		{
			return await _usersCollection.Find(x => x.UserName == username).FirstOrDefaultAsync();
		}

		public async Task CreateUserAsync(User user)
		{
			await _usersCollection.InsertOneAsync(user);
		}
	}
}
