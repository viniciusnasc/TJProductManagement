using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TJ.ProductManagement.Domain.Entities
{
    public abstract class BaseEntity 
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; private set; }

        protected BaseEntity()
        {
            Id = new ObjectId();
        }
    }
}
