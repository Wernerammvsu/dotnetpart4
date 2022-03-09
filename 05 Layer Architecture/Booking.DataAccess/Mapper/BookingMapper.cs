using Booking.DataAccess.Models;

namespace Booking.DataAccess.Mapper;

public class BookingMapper : IBookingMapper
{
	public Domain.Entity.Booking Map(BookingDAL bookingDAL)
	{
		return new Domain.Entity.Booking(bookingDAL.Id,
			bookingDAL.Comment,
			bookingDAL.FromUtc,
			bookingDAL.ToUtc,
			bookingDAL.UserId,
			roomId: bookingDAL.RoomId);
	}

	public BookingDAL Map(Domain.Entity.Booking booking)
	{
		BookingDAL bookingDal = new BookingDAL
		{
			Comment = booking.Comment,
			FromUtc = booking.FromUtc,
			ToUtc = booking.ToUtc,
			UserId = booking.UserId
		};
		if (booking.Id is not null)
			bookingDal.Id = booking.Id.Value;
		return bookingDal;
	}
}
