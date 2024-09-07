using MongoDB.Bson;
using MongoDB.Driver;
using TJ.ProductManagement.Data.MongoConfiguration;
using TJ.ProductManagement.Domain.Entities;
using TJ.ProductManagement.Domain.Interfaces.Repositories;

namespace TJ.ProductManagement.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        protected IMongoCollection<Product> _dbCollection;

        public ProductRepository(IMongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _dbCollection = database.GetCollection<Product>(nameof(Product));
        }

        public async Task<Product> GetById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<Product>.Filter.Eq(doc => doc.Id, objectId);
            return await _dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public IQueryable<Product> GetAll()
        => _dbCollection.AsQueryable();

        public async Task Add(Product entity)
        => await _dbCollection.InsertOneAsync(entity);

        public async Task Update(Product entity)
        {
            var filter = Builders<Product>.Filter.Eq(doc => doc.Id, entity.Id);
            await _dbCollection.FindOneAndReplaceAsync(filter, entity);
        }

        public async Task Remove(Product entity)
        {
            var filter = Builders<Product>.Filter.Eq(ent => ent.Id, entity.Id);
            await _dbCollection.DeleteOneAsync(filter);
        }

        public async Task<long> CountItems()
        {
            var filter = Builders<Product>.Filter.Empty;
            return _dbCollection.CountDocuments(filter);
        }
    }
}
