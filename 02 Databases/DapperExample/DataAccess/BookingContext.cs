using EFCoreExample.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace EFCoreExample.DataAccess
{
	public class BookingContext : DbContext
	{
		public BookingContext(DbContextOptions options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Booking> Bookings { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Booking>()
				.HasOne(x => x.User)
				.WithMany(u => u.Bookings)
				.HasForeignKey(b => b.UserId);
		}
	}
}
