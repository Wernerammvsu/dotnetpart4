using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbExample.DataAccess.Entity
{
	public class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string UserName { get; set; }
	}
}

