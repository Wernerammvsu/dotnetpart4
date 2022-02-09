using MongoDB.Driver;
using MongoDbExample.Configuration;
using MongoDbExample.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDbExample.DataAccess
{
	public class BookingRespository
	{
		private readonly IMongoCollection<Booking> _bookingsCollection;

		public BookingRespository(BookingDatabaseConfiguration bookingDatabaseConfiguration)
		{
			var mongoClient = new MongoClient(bookingDatabaseConfiguration.ConnectionString);

			IMongoDatabase mongoDatabase =
				mongoClient.GetDatabase(bookingDatabaseConfiguration.DatabaseName);

			_bookingsCollection = mongoDatabase.GetCollection<Booking>("Bookings");
		}

		public async Task<IEnumerable<Booking>> GetAllAsync()
		{
			return await _bookingsCollection.Find(_ => true).ToListAsync();
		}

		public async Task<IEnumerable<Booking>> FindAll(Expression<Func<Booking, bool>> filter)
		{
			return await _bookingsCollection.Find(filter).ToListAsync();
		}

		public async Task CreateAsync(Booking newBooking)
		{
			await _bookingsCollection.InsertOneAsync(newBooking);
		}
	}
}
