using DtoValidation.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DtoValidation.DataAccess;

public class BookingContext : DbContext
{
	public BookingContext(DbContextOptions options) : base(options) { }

	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Booking> Bookings { get; set; } = null!;
	public DbSet<Room> Rooms { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		ConfigureBooking(modelBuilder);
		ConfigureUser(modelBuilder);
		ConfigureRoom(modelBuilder);
	}

	private static void ConfigureUser(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
						.HasIndex(b => b.UserName);
		modelBuilder.Entity<User>()
			.Property(b => b.UserName)
			.IsRequired();
	}

	private static void ConfigureRoom(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Room>()
						.HasIndex(b => b.RoomName);
		modelBuilder.Entity<Room>()
			.Property(b => b.RoomName)
			.IsRequired();
	}

	private static void ConfigureBooking(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Booking>()
			.HasOne(x => x.User)
			.WithMany(u => u.Bookings)
			.HasForeignKey(b => b.UserId)
			.IsRequired(true);
		modelBuilder.Entity<Booking>()
			.HasOne(x => x.Room)
			.WithMany(u => u.Bookings)
			.HasForeignKey(b => b.RoomId)
			.IsRequired(true);
		modelBuilder.Entity<Booking>()
			.Property(b => b.RoomId)
			.HasDefaultValue(Booking.DefaultRoomId);
		modelBuilder.Entity<Booking>()
			.Property(b => b.FromUtc)
			.IsRequired(true);
		modelBuilder.Entity<Booking>()
			.Property(b => b.ToUtc)
			.IsRequired(true);
		modelBuilder.Entity<Booking>()
			.HasIndex(b => b.FromUtc);
		modelBuilder.Entity<Booking>()
			.HasIndex(b => b.ToUtc);
	}
}
