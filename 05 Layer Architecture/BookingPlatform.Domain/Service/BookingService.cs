using BookingPlatform.Domain.Entity;
using BookingPlatform.Domain.Persistence;

namespace BookingPlatform.Domain.Service
{
	public class BookingService : IBookingService
	{
		private readonly IBookingRepository _bookingRepository;
		private readonly ITimeProvider _timeProvider;

		public BookingService(IBookingRepository bookingRepository,
			ITimeProvider timeProvider)
		{
			_bookingRepository = bookingRepository;
			_timeProvider = timeProvider;
		}

		public async Task<IEnumerable<Entity.Booking>> FindBookingsWithDefaultRoomAsync()
		{
			return await _bookingRepository.FindBookingsByRoomIdAsync(Booking.DefaultRoomId);
		}

		public async Task<IEnumerable<Entity.Booking>> FindBookingsByRoomIdAsync(int roomId)
		{
			return await _bookingRepository.FindBookingsByRoomIdAsync(roomId);
		}

		public async Task<Entity.Booking> CreateBookingAsync(Entity.Booking booking)
		{
			Entity.Booking? alreadyCreatedBooking =
				await _bookingRepository.FindActiveBookingInRoomAsync(booking.RoomId, booking.FromUtc, booking.ToUtc);
			if (alreadyCreatedBooking is not null)
				throw new Exception("Booking for this time has already been created");
			return await _bookingRepository.SaveAsync(booking);
		}

		public async Task<IEnumerable<Entity.Booking>> FindAllActiveAsync()
		{
			return await _bookingRepository.FindWithEndEarlierThanAsync(_timeProvider.UtcNow);
		}
	}
}
