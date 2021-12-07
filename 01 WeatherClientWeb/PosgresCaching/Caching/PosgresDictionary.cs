using Npgsql;
using System;
using System.Threading.Tasks;

namespace RedisCaching.Caching
{
	public class PosgresDictionary
	{
		private const string ConnectionString = "Host=localhost;Username=postgres;Password=mysecretpassword";

		private bool _tableChecked = false;

		public async Task<string> GetAsync(string key)
		{
			// Open connection
			await using var conn = new NpgsqlConnection(ConnectionString);
			await conn.OpenAsync();

			await using (var cmd = new NpgsqlCommand("INSERT INTO data (some_field) VALUES (@p)", conn))
			{
				cmd.Parameters.AddWithValue("p", "Hello world");
				await cmd.ExecuteNonQueryAsync();
			}


			using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
			using (var reader = await cmd.ExecuteReaderAsync())
				while (await reader.ReadAsync())
					Console.WriteLine(reader.GetString(0));

			return null;
		}

		public async Task<bool> SetAsync(string key, string value)
		{
			return true;
		}

		private void CheckTable()
		{
			if (_tableChecked)
				return;

			var commandString = @"CREATE TABLE IF NOT EXISTS app_user (
  cacheKey varchar(100) NOT NULL,
  cacheValue varchar(max) NOT NULL,
  PRIMARY KEY (cacheKey)
)";
			using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
			using (var reader = await cmd.ExecuteReaderAsync())
		}
	}
}
