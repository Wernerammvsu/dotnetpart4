namespace EFCoreExample.DataAccess.Entity
{
	public class Booking
	{
		public int Id { get; set; }
		public string Comment { get; set; }
		
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
