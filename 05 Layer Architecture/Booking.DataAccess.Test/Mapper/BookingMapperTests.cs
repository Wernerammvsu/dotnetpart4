using Booking.DataAccess.Mapper;
using Booking.DataAccess.Models;
using FluentAssertions;
using System;
using Xunit;

namespace Booking.DataAccess.Test.Mapper
{
	public class BookingMapperTests
	{
		private const int BookingId = 1;
		private const int RoomId = 2;
		private const int UserId = 3;
		private const string Comment = "Comment";
		private readonly DateTime FromUtc = new DateTime(2022, 03, 09);
		private readonly DateTime ToUtc = new DateTime(2022, 03, 10);

		[Fact]
		public void MapsDaoToEntity()
		{
			var mapper = new BookingMapper();
			var bookingDal = new BookingDAL
			{
				Id = BookingId,
				Comment = Comment,
				FromUtc = FromUtc,
				ToUtc = ToUtc,
				RoomId = RoomId,
				UserId = UserId
			};

			Domain.Entity.Booking result =
				mapper.Map(bookingDal);

			Assert.Equal(result.Id, BookingId);
			Assert.Equal(result.Comment, Comment);
			Assert.Equal(result.FromUtc, FromUtc);
			Assert.Equal(result.ToUtc, ToUtc);
			Assert.Equal(result.RoomId, RoomId);
			Assert.Equal(result.UserId, UserId);
		}

		[Fact]
		public void UsingFluentAssertions()
		{
			var mapper = new BookingMapper();
			var bookingDal = new BookingDAL
			{
				Id = BookingId,
				Comment = Comment,
				FromUtc = FromUtc,
				ToUtc = ToUtc,
				RoomId = RoomId,
				UserId = UserId
			};

			Domain.Entity.Booking result =
				mapper.Map(bookingDal);

			var expectedBooking = new Domain.Entity.Booking(
				id: BookingId,
				comment: Comment,
				fromUtc: FromUtc,
				toUtc: ToUtc,
				userId: UserId,
				roomId: RoomId);
			result.Should().BeEquivalentTo(expectedBooking);
		}
	}
}
