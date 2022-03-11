using Booking.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.DataAccess;

public class BookingContext : DbContext
{
	public BookingContext(DbContextOptions options) : base(options) { }

	public DbSet<UserDAL> Users { get; set; } = null!;
	public DbSet<BookingDAL> Bookings { get; set; } = null!;
	public DbSet<RoomDAL> Rooms { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		ConfigureBooking(modelBuilder);
		ConfigureUser(modelBuilder);
		ConfigureRoom(modelBuilder);
	}

	private static void ConfigureUser(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserDAL>()
						.HasIndex(b => b.UserName);
		modelBuilder.Entity<UserDAL>()
			.Property(b => b.UserName)
			.IsRequired();
	}

	private static void ConfigureRoom(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<RoomDAL>()
						.HasIndex(b => b.RoomName);
		modelBuilder.Entity<RoomDAL>()
			.Property(b => b.RoomName)
			.IsRequired();
	}

	private static void ConfigureBooking(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<BookingDAL>()
			.HasOne(x => x.User)
			.WithMany(u => u.Bookings)
			.HasForeignKey(b => b.UserId)
			.IsRequired(true);
		modelBuilder.Entity<BookingDAL>()
			.HasOne(x => x.Room)
			.WithMany(u => u.Bookings)
			.HasForeignKey(b => b.RoomId)
			.IsRequired(true);
		modelBuilder.Entity<BookingDAL>()
			.Property(x => x.RoomId)
			.HasDefaultValue(Domain.Entity.Booking.DefaultRoomId);
		modelBuilder.Entity<BookingDAL>()
			.Property(b => b.FromUtc)
			.IsRequired(true);
		modelBuilder.Entity<BookingDAL>()
			.Property(b => b.ToUtc)
			.IsRequired(true);
		modelBuilder.Entity<BookingDAL>()
			.HasIndex(b => b.FromUtc);
		modelBuilder.Entity<BookingDAL>()
			.HasIndex(b => b.ToUtc);
	}
}
