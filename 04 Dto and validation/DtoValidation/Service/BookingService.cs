using DtoValidation.DataAccess;
using DtoValidation.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DtoValidation.Service
{
	public class BookingService
	{
		private readonly BookingContext _bookingContext;

		public BookingService(BookingContext bookingContext)
		{
			_bookingContext = bookingContext;
		}

		public async Task<Booking> CreateBookingAsync(Booking booking)
		{
			Booking? alreadyCreatedBooking = await _bookingContext
				.Bookings
				.FirstOrDefaultAsync(b =>
					b.RoomId == booking.RoomId
					&& (b.FromUtc <= booking.FromUtc
						&& b.ToUtc >= booking.FromUtc
						|| b.FromUtc <= booking.ToUtc
						&& b.ToUtc >= booking.ToUtc
						|| b.FromUtc == booking.FromUtc
						&& b.ToUtc == booking.ToUtc));
			if (alreadyCreatedBooking is not null)
				throw new Exception("Booking for this time has already been created");

			_bookingContext.Add(booking);
			await _bookingContext.SaveChangesAsync();
			return booking;
		}
	}
}
