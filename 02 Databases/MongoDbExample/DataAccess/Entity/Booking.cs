using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MongoDbExample.DataAccess.Entity
{
	public class Booking
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string Comment { get; set; }
		public DateTime FromUtc { get; set; }
		public DateTime ToUtc { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; }
	}
}
