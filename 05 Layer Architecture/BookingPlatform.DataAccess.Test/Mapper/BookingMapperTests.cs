using BookingPlatform.DataAccess.Mapper;
using BookingPlatform.DataAccess.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BookingPlatform.DataAccess.Test.Mapper
{
	public class BookingMapperTests
	{
		private const int BookingId = 1;
		private const int RoomId = 2;
		private const string Username = "Username";
		private const string Comment = "Comment";
		private readonly DateTime FromUtc = new DateTime(2022, 03, 09);
		private readonly DateTime ToUtc = new DateTime(2022, 03, 10);

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
				Username = Username
			};

			Domain.Entity.Booking result =
				mapper.Map(bookingDal);

			var expectedBooking = new Domain.Entity.Booking(
				id: BookingId,
				comment: Comment,
				fromUtc: FromUtc,
				toUtc: ToUtc,
				username: Username,
				roomId: RoomId);
			result.Should().BeEquivalentTo(expectedBooking);
		}
	}
}
