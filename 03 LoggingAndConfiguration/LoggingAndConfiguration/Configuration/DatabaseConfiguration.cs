namespace MongoDbExample.Configuration
{
	public class DatabaseConfiguration
	{
		public string ConnectionString { get; set; }
		public int UserNameMinLength { get; set; }
		public int MinTimeSpanForBookingInMinutes { get; set; }

	}
}
