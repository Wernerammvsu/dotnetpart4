namespace MongoDbExample.Configuration
{
	public class BookingDatabaseConfiguration
	{
		public readonly string HostName;
		public readonly string Username;
		public readonly string Password;
		public readonly string DatabaseName;
		public readonly string ConnectionString;

		public BookingDatabaseConfiguration(string connectionString, string databaseName)
		{
			ConnectionString = connectionString;
			DatabaseName = databaseName;
		}

		public BookingDatabaseConfiguration(string hostName,
			string username,
			string password,
			string databaseName)
		{
			HostName = hostName;
			Username = username;
			Password = password;
			DatabaseName = databaseName;
		}
	}
}
