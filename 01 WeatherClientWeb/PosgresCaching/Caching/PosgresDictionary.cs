using Npgsql;
using System.Threading.Tasks;

namespace PostgresCaching.Caching
{
	public class PosgresDictionary
	{
		private const string ConnectionString = "Host=localhost;Username=postgres;Password=mysecretpassword";

		private bool _tableChecked = false;

		public async Task<string> GetAsync(string key)
		{
			NpgsqlConnection connection = await OpenConnectionAndCheckTableAsync();

			using var cmd = new NpgsqlCommand("SELECT cacheValue FROM weather_cache WHERE cacheKey=@key", connection);
			cmd.Parameters.AddWithValue("key", key);
			using var reader = await cmd.ExecuteReaderAsync();
			while (await reader.ReadAsync())
				return reader.GetString(0);
			return null;
		}

		public async Task<bool> SetAsync(string key, string value)
		{
			NpgsqlConnection connection = await OpenConnectionAndCheckTableAsync();

			await using (var cmd = new NpgsqlCommand("INSERT INTO weather_cache (cacheKey, cacheValue) VALUES (@key, @value)", connection))
			{
				cmd.Parameters.AddWithValue("key", key);
				cmd.Parameters.AddWithValue("value", value);
				await cmd.ExecuteNonQueryAsync();
			}

			return true;
		}

		private async Task<NpgsqlConnection> OpenConnectionAndCheckTableAsync()
		{
			// Open connection
			var connection = new NpgsqlConnection(ConnectionString);
			await connection.OpenAsync();

			await CheckTableAsync(connection);
			return connection;
		}

		private async Task CheckTableAsync(NpgsqlConnection npgsqlConnection)
		{
			if (_tableChecked)
				return;

			// H/W: Implement sliding expiration using postgres
			var commandString = @"CREATE TABLE IF NOT EXISTS weather_cache (
  cacheKey varchar(100) NOT NULL,
  cacheValue varchar NOT NULL,
  PRIMARY KEY (cacheKey)
)";
			using (var cmd = new NpgsqlCommand(commandString, npgsqlConnection))
				await cmd.ExecuteNonQueryAsync();

			_tableChecked = true;
		}
	}
}
