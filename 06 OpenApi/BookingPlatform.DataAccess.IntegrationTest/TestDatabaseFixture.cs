using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.DataAccess.IntegrationTest
{
    public class TestDatabaseFixture
	{
		private const string ConnectionString = "Host=localhost;Database=booking;Username=postgres;Password=mysecretpassword";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }
                    _databaseInitialized = true;
                }
            }
        }

        public BookingContext CreateContext()
            => new BookingContext(
                new DbContextOptionsBuilder<BookingContext>()
                    .UseNpgsql(ConnectionString)
                    .Options);
    }
}
