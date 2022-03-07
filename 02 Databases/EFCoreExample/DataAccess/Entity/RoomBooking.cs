namespace EFCoreExample.DataAccess.Entity
{
    public class RoomBooking
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
