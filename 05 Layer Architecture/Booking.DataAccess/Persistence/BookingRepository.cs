using Booking.DataAccess;
using Booking.DataAccess.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Booking.Domain.Persistence
{
	public class BookingRepository : IBookingRepository
	{
		private readonly BookingContext _bookingContext;
		private readonly IBookingMapper _bookingMapper;

		public BookingRepository(BookingContext bookingContext,
			IBookingMapper bookingMapper)
		{
			_bookingContext = bookingContext;
			_bookingMapper = bookingMapper;
		}

		public async Task<IEnumerable<Entity.Booking>> FindBookingsByRoomIdAsync(int roomId)
		{
			List<DataAccess.Models.BookingDAL> bookings =
				await _bookingContext.Bookings
					.Where(b => b.RoomId == roomId).ToListAsync();
			return bookings.Select(b => _bookingMapper.Map(b)).ToList();
		}

		public async Task<Entity.Booking?> FindActiveBookingInRoomAsync(int roomId, DateTime fromUtc, DateTime toUtc)
		{
			fromUtc = DateTime.SpecifyKind(fromUtc, DateTimeKind.Utc);
			toUtc = DateTime.SpecifyKind(toUtc, DateTimeKind.Utc);

			DataAccess.Models.BookingDAL? booking = await _bookingContext
				.Bookings
				.FirstOrDefaultAsync(b =>
					b.RoomId == roomId
					&& b.FromUtc <= toUtc
					&& b.ToUtc >= fromUtc);
			if (booking is null)
				return null;
			return _bookingMapper.Map(booking);
		}

		public async Task<IEnumerable<Entity.Booking>> FindWithEndEarlierThanAsync(DateTime dateTimeUtc)
		{
			dateTimeUtc = DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc);
			List<DataAccess.Models.BookingDAL> bookings = await _bookingContext
				.Bookings
				.Where(b => b.ToUtc < dateTimeUtc)
				.ToListAsync();
			return bookings.Select(b => _bookingMapper.Map(b)).ToList();
		}

		public async Task<Entity.Booking> SaveAsync(Entity.Booking booking)
		{
			DataAccess.Models.BookingDAL bookingDal = _bookingMapper.Map(booking);
			_bookingContext.Bookings.Add(bookingDal);
			await _bookingContext.SaveChangesAsync();
			return _bookingMapper.Map(bookingDal);
		}
	}
}
