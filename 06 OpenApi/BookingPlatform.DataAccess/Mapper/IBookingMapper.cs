using BookingPlatform.DataAccess.Models;

namespace BookingPlatform.DataAccess.Mapper
{
	public interface IBookingMapper
	{
		Domain.Entity.Booking Map(BookingDAL bookingDAL);
		BookingDAL Map(Domain.Entity.Booking booking);
	}
}