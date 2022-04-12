using BookingPlatform.DataAccess.Mapper;
using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.DataAccess.Persistence
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

        public async Task<IEnumerable<Booking>> FindBookingsByRoomIdAsync(int roomId)
        {
            List<BookingDAL> bookings =
                await _bookingContext.Bookings
                    .Where(b => b.RoomId == roomId).ToListAsync();
            return bookings.Select(b => _bookingMapper.Map(b)).ToList();
        }

        public async Task<Booking?> FindActiveBookingInRoomAsync(int roomId, DateTime fromUtc, DateTime toUtc)
        {
            fromUtc = DateTime.SpecifyKind(fromUtc, DateTimeKind.Utc);
            toUtc = DateTime.SpecifyKind(toUtc, DateTimeKind.Utc);

            Models.BookingDAL? booking = await _bookingContext
                .Bookings
                .FirstOrDefaultAsync(b =>
                    b.RoomId == roomId
                    && b.FromUtc <= toUtc
                    && b.ToUtc >= fromUtc);
            if (booking is null)
                return null;
            return _bookingMapper.Map(booking);
        }

        public async Task<IEnumerable<Booking>> FindWithEndEarlierThanAsync(DateTime dateTimeUtc)
        {
            dateTimeUtc = DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc);
            List<BookingDAL> bookings = await _bookingContext
                .Bookings
                .Where(b => b.ToUtc < dateTimeUtc)
                .ToListAsync();
            return bookings.Select(b => _bookingMapper.Map(b)).ToList();
        }

        public async Task<Booking> SaveAsync(Booking booking)
        {
            BookingDAL bookingDal = _bookingMapper.Map(booking);
            _bookingContext.Bookings.Add(bookingDal);
            await _bookingContext.SaveChangesAsync();
            return _bookingMapper.Map(bookingDal);
        }
    }
}
