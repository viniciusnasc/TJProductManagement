using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TJ.ProductManagement.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public DateTime CreateAt { get; private set; }
        public DateTime UpdateAt { get; private set; }

        public Product(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }

        public void Update(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
            UpdateAt = DateTime.Now;
        }
    }
}
