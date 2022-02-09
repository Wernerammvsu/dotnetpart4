using Dapper;
using DapperExample.DataAccess.Entity;
using Npgsql;
using System.Threading.Tasks;

namespace DapperExample.DataAccess
{
	public class UserRepository
	{
		private const string ConnectionString = "Host=localhost;Database=postgres;Username=postgres;Password=mysecretpassword";

		public async Task<User> FindUserAsync(string username)
		{
			string sql = "SELECT u.\"Id\", u.\"UserName\" FROM \"Users\" AS u WHERE u.\"UserName\" LIKE @username";

			using (var connection = new NpgsqlConnection(ConnectionString))
			{
				return await connection.QueryFirstAsync<User>(sql, new { username = username });
			}
		}

		public async Task CreateUserAsync(User user)
		{
			string sql = "INSERT INTO \"Users\" (\"UserName\") VALUES(@username) RETURNING \"Id\";";

			using (var connection = new NpgsqlConnection(ConnectionString))
			{
				int userId = await connection.QueryFirstAsync<int>(sql, new { username = user.UserName });
				user.Id = userId;
			}
		}
	}
}
