using EFCoreExample.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreExample.DataAccess
{
	public class BookingContext : DbContext
	{
		public BookingContext(DbContextOptions options) : base(options) { }

		public DbSet<User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.EnableSensitiveDataLogging()
				.LogTo(System.Console.WriteLine);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
