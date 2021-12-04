using StackExchange.Redis;
using System.Threading.Tasks;

namespace RedisCaching.Caching
{
	public class RedisDictionary
	{
		private readonly Task<ConnectionMultiplexer> _connectionMultiplexerTask;

		public RedisDictionary()
		{
			_connectionMultiplexerTask = ConnectionMultiplexer.ConnectAsync("localhost");
		}

		public async Task<string> GetAsync(string key)
		{
			ConnectionMultiplexer connection = await _connectionMultiplexerTask;
			IDatabase db = connection.GetDatabase();
			RedisValue result = await db.StringGetAsync(key);
			if (!result.HasValue)
				return null;
			return result.ToString();
		}

		public async Task<bool> SetAsync(string key, string value)
		{
			ConnectionMultiplexer connection = await _connectionMultiplexerTask;
			IDatabase db = connection.GetDatabase();
			return await db.StringSetAsync(key, value);
		}
	}
}
