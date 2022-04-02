using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;

namespace BookingPlatform.DataAccess.Mapper
{
    public interface IRoomMapper
    {
        Room Map(RoomDAL bookingDAL);
        RoomDAL Map(Room booking);
    }
}
