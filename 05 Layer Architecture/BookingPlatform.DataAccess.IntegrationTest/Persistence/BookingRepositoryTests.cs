using BookingPlatform.DataAccess.Mapper;
using BookingPlatform.DataAccess.Models;
using BookingPlatform.DataAccess.Persistence;
using BookingPlatform.Domain.Entity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BookingPlatform.DataAccess.IntegrationTest.Persistence
{
	public class BookingRepositoryTests : IClassFixture<TestDatabaseFixture>
	{
		private readonly BookingRepository _bookingRepository;

		private const string DefaultUsername = "Username";
		private const int DefaultBookingId = 2;
		private const int DefaultRoomId = 3;

		private readonly DateTime FromUtc = new DateTime(2022, 03, 16, 22, 14, 00, DateTimeKind.Utc);
		private readonly DateTime ToUtc = new DateTime(2022, 03, 17, 22, 14, 00, DateTimeKind.Utc);

		private readonly Mock<IBookingMapper> _bookingMapperMock;
		private readonly BookingContext _context;

		public BookingRepositoryTests(TestDatabaseFixture fixture)
		{
			_bookingMapperMock = new Mock<IBookingMapper>(MockBehavior.Strict);
			_context = fixture.CreateContext();
			_bookingRepository = new BookingRepository(_context, _bookingMapperMock.Object);
		}

		[Fact]
		public async Task FindsBookingEarlierThanDate()
		{
			using var transaction = _context.Database.BeginTransaction();
			await CreateDefaultUserAndRoomAsync();
			var bookingDal = CreateDefaultDal(DefaultBookingId);
			var expectedBooking = CreateDefault(DefaultBookingId);
			_context.Bookings.Add(bookingDal);
			await _context.SaveChangesAsync();
			_bookingMapperMock.Setup(bm => bm.Map(It.Is<BookingDAL>(b => b.Id == DefaultBookingId)))
				.Returns(expectedBooking);
			// Sometimes it's useful
			_context.ChangeTracker.Clear();

			System.Collections.Generic.IEnumerable<Domain.Entity.Booking> list =
				await _bookingRepository
					.FindWithEndEarlierThanAsync(new DateTime(2022, 03, 18));

			list.Should().BeEquivalentTo(new[] { expectedBooking });
		}

		[Fact]
		public async Task FindsBookingsByRoomIdAsync()
		{
			using var transaction = _context.Database.BeginTransaction();
			await CreateDefaultUserAndRoomAsync();
			var bookingDal = CreateDefaultDal(DefaultBookingId);
			var expectedBooking = CreateDefault(DefaultBookingId);
			_context.Bookings.Add(bookingDal);
			await _context.SaveChangesAsync();
			_bookingMapperMock.Setup(bm => bm.Map(It.Is<BookingDAL>(b => b.Id == DefaultBookingId)))
				.Returns(expectedBooking);

			System.Collections.Generic.IEnumerable<Booking> list =
				await _bookingRepository
					.FindBookingsByRoomIdAsync(DefaultRoomId);

			list.Should().BeEquivalentTo(new[] { expectedBooking });
		}

		[Fact]
		public async Task FindsActiveBookingInRoomAsync()
		{
			using var transaction = _context.Database.BeginTransaction();
			await CreateDefaultUserAndRoomAsync();
			var bookingDal = CreateDefaultDal(DefaultBookingId);
			var expectedBooking = CreateDefault(DefaultBookingId);
			_context.Bookings.Add(bookingDal);
			await _context.SaveChangesAsync();
			_bookingMapperMock.Setup(bm => bm.Map(It.Is<BookingDAL>(b => b.Id == DefaultBookingId)))
				.Returns(expectedBooking);

			System.Collections.Generic.IEnumerable<Domain.Entity.Booking> list =
				await _bookingRepository
					.FindBookingsByRoomIdAsync(DefaultRoomId);

			list.Should().BeEquivalentTo(new[] { expectedBooking });
		}

		[Fact]
		public async Task FindsBookingsByRoomIdAsyncReturnsNullWhenNothingIsFound()
		{
			Domain.Entity.Booking? result =
				await _bookingRepository
					.FindActiveBookingInRoomAsync(DefaultRoomId, FromUtc, ToUtc);

			result.Should().BeNull();
		}

		[Fact]
		public async Task FindsBookingsByRoomIdAsyncReturnsNullWhenDataIsPresent()
		{
			using var transaction = _context.Database.BeginTransaction();
			await CreateDefaultUserAndRoomAsync();
			var bookingDal = CreateDefaultDal(DefaultBookingId);
			var expectedBooking = CreateDefault(DefaultBookingId);
			_context.Bookings.Add(bookingDal);
			await _context.SaveChangesAsync();
			_bookingMapperMock.Setup(bm => bm.Map(It.Is<BookingDAL>(b => b.Id == DefaultBookingId)))
				.Returns(expectedBooking);

			Domain.Entity.Booking? result =
				await _bookingRepository
					.FindActiveBookingInRoomAsync(DefaultRoomId, FromUtc, ToUtc);

			result.Should().BeEquivalentTo(expectedBooking);
		}

		[Fact]
		public async Task SavesBookingToDb()
		{
			using var transaction = _context.Database.BeginTransaction();
			await CreateDefaultUserAndRoomAsync();
			var bookingDal = CreateDefaultDal(DefaultBookingId);
			var booking = CreateDefault(DefaultBookingId);
			_bookingMapperMock.Setup(bm => bm.Map(It.Is<Domain.Entity.Booking>(b => b.Id == DefaultBookingId)))
				.Returns(bookingDal);
			_bookingMapperMock.Setup(bm => bm.Map(It.Is<BookingDAL>(b => b.Id == DefaultBookingId)))
				.Returns(booking);

			await _bookingRepository
				.SaveAsync(booking);
			BookingDAL? bookingDalInDb =
				await _context.Bookings.FirstOrDefaultAsync(b => b.Id == DefaultBookingId);

			bookingDalInDb.Should().BeEquivalentTo(bookingDal);
		}

		private async Task CreateDefaultUserAndRoomAsync()
		{
			_context.Users.Add(new UserDAL { Username = DefaultUsername, PasswordHash = "Hash" });
			_context.Rooms.Add(new RoomDAL { Id = DefaultRoomId, RoomName = "RoomName" });
			await _context.SaveChangesAsync();
		}

		private Domain.Entity.Booking CreateDefault(int? id)
		{
			return new Domain.Entity.Booking(id: id,
				comment: "comment",
				fromUtc: FromUtc,
				toUtc: ToUtc,
				username: DefaultUsername,
				roomId: DefaultRoomId);
		}

		private BookingDAL CreateDefaultDal(int? id = null)
		{
			return new BookingDAL
			{
				Id = id ?? 0,
				Comment = "comment",
				FromUtc = FromUtc,
				ToUtc = ToUtc,
				Username = DefaultUsername,
				RoomId = DefaultRoomId
			};
		}
	}
}
