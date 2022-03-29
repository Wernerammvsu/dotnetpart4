using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;

namespace BookingPlatform.DataAccess.Mapper;

public class BookingMapper : IBookingMapper
{
	public Booking Map(BookingDAL bookingDAL)
	{
		return new Booking(bookingDAL.Id,
			bookingDAL.Comment,
			bookingDAL.FromUtc,
			bookingDAL.ToUtc,
			bookingDAL.Username,
			roomId: bookingDAL.RoomId);
	}

	public BookingDAL Map(Booking booking)
	{
		BookingDAL bookingDal = new BookingDAL
		{
			Comment = booking.Comment,
			FromUtc = booking.FromUtc,
			ToUtc = booking.ToUtc,
			Username = booking.Username
		};
		if (booking.Id is not null)
			bookingDal.Id = booking.Id.Value;
		return bookingDal;
	}
}
