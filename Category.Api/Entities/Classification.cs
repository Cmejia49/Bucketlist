using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Category.Api.Entities
{
    public class Classification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
