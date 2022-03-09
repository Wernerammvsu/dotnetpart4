using Booking.Domain.Persistence;
using Booking.Domain.Service;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Booking.Domain.Test.Service
{
	public class BookingServiceTest
	{
		[Fact]
		public async Task FindsBookingsByRoomIdAsync()
		{
			const int roomId = 1;
			var moq = new Mock<IBookingRepository>();
			var service = new BookingService(moq.Object,
				Mock.Of<ITimeProvider>(MockBehavior.Strict));
			var expectedResult = Array.Empty<Entity.Booking>();
			moq.Setup(m => m.FindBookingsByRoomIdAsync(roomId))
				.ReturnsAsync(expectedResult);

			var result = await service.FindBookingsByRoomIdAsync(roomId);

			result.Should().BeSameAs(expectedResult);
		}
	}
}
