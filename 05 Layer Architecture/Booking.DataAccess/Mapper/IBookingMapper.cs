using Booking.DataAccess.Models;

namespace Booking.DataAccess.Mapper
{
	public interface IBookingMapper
	{
		Domain.Entity.Booking Map(BookingDAL bookingDAL);
		BookingDAL Map(Domain.Entity.Booking booking);
	}
}