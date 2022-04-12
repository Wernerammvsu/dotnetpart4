using System.Collections.ObjectModel;

namespace BookingPlatform.Domain.Entity;

public class Room
{
    public int Id { get; set; }
    public string RoomName { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; }

    public Room()
    {
        Bookings = new Collection<Booking>();
    }
}
