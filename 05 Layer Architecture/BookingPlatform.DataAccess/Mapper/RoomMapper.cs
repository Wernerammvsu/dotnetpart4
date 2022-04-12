using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;

namespace BookingPlatform.DataAccess.Mapper
{
    public class RoomMapper : IRoomMapper
    {
        private readonly IBookingMapper _bookingMapper;

        public RoomMapper(IBookingMapper bookingMapper)
        {
            _bookingMapper = bookingMapper;
        }

        public Room Map(RoomDAL roomDAL)
        {
            return new Room
            {
                Id = roomDAL.Id,
                RoomName = roomDAL.RoomName,
                Bookings = roomDAL.Bookings.Select(b => _bookingMapper.Map(b)).ToArray()
            };
        }

        public RoomDAL Map(Room room)
        {
            return new RoomDAL
            {
                Id = room.Id,
                RoomName = room.RoomName,
                Bookings = room.Bookings.Select(b => _bookingMapper.Map(b)).ToArray()
            };
        }
    }
}
