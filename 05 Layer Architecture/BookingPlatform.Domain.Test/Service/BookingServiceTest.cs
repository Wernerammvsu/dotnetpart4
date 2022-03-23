using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Persistence;
using BookingPlatform.Domain.Service;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookingPlatform.Domain.Test.Service
{
	public class BookingServiceTest
	{
		private readonly BookingService _bookingService;

		private readonly Mock<IBookingRepository> _bookingRepositoryMock;
		private readonly Mock<ITimeProvider> _timeProviderMock;

		public BookingServiceTest()
		{
			_bookingRepositoryMock = new Mock<IBookingRepository>(MockBehavior.Strict);
			_timeProviderMock = new Mock<ITimeProvider>(MockBehavior.Strict);
			_bookingService = new BookingService(_bookingRepositoryMock.Object, _timeProviderMock.Object);
		}

		[Fact]
		public async Task FindsBookingsByRoomIdAsync()
		{
			const int roomId = 1;

			var expectedResult = Array.Empty<Entity.Booking>();
			_bookingRepositoryMock.Setup(m => m.FindBookingsByRoomIdAsync(roomId))
				.ReturnsAsync(expectedResult);

			var result = await _bookingService.FindBookingsByRoomIdAsync(roomId);

			result.Should().BeSameAs(expectedResult);
		}

		[Fact]
		public async Task FindsBookingsWithDefaultRoomAsync()
		{
			var expectedResult = Array.Empty<Entity.Booking>();
			_bookingRepositoryMock.Setup(m => m.FindBookingsByRoomIdAsync(Booking.DefaultRoomId))
				.ReturnsAsync(expectedResult);

			IEnumerable<Entity.Booking> result = await _bookingService.FindBookingsWithDefaultRoomAsync();

			result.Should().BeSameAs(expectedResult);
		}

		[Fact]
		public async Task FindAllActiveRoomsEarlierThanUtcNowAsync()
		{
			var dateTimeUtcNow = new DateTime(2022, 03, 16);
			_timeProviderMock.Setup(tp => tp.UtcNow).Returns(dateTimeUtcNow);

			var expectedResult = Array.Empty<Entity.Booking>();
			_bookingRepositoryMock.Setup(m => m.FindWithEndEarlierThanAsync(dateTimeUtcNow))
				.ReturnsAsync(expectedResult);

			IEnumerable<Entity.Booking> result = await _bookingService.FindAllActiveAsync();

			result.Should().BeSameAs(expectedResult);
		}

		[Fact]
		public async Task CreateBookingAsync_ThrownsExceptionWhenBookingAlreadyExists()
		{
			var fromUtc = new DateTime(2022, 03, 16);
			var toUtc = new DateTime(2022, 03, 16);
			int roomId = 3;
			var booking = new Entity.Booking(id: 1,
				comment: "comment",
				fromUtc: fromUtc,
				toUtc: toUtc,
				username: "username",
				roomId: roomId);
			_bookingRepositoryMock.Setup(br => br.FindActiveBookingInRoomAsync(roomId, fromUtc, toUtc))
				.ReturnsAsync(booking);

			Func<Task<Entity.Booking>>? action = async () => await _bookingService.CreateBookingAsync(booking);

			await action.Should()
				.ThrowAsync<Exception>()
				.WithMessage("Booking for this time has already been created");
		}

		[Fact]
		public async Task CreateBookingAsync_SavesBooking()
		{
			var fromUtc = new DateTime(2022, 03, 16);
			var toUtc = new DateTime(2022, 03, 16);
			int roomId = 3;
			var booking = new Entity.Booking(id: 1,
				comment: "comment",
				fromUtc: fromUtc,
				toUtc: toUtc,
				username: "username",
				roomId: roomId);
			_bookingRepositoryMock.Setup(br => br.FindActiveBookingInRoomAsync(roomId, fromUtc, toUtc))
				.ReturnsAsync((Entity.Booking?)null);
			_bookingRepositoryMock.Setup(br => br.SaveAsync(booking))
				.ReturnsAsync(booking);

			await _bookingService.CreateBookingAsync(booking);

			_bookingRepositoryMock.Verify(r => r.SaveAsync(booking), Times.Once);
		}
	}
}
