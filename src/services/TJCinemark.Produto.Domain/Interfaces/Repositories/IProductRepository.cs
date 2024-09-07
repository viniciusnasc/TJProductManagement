using TJ.ProductManagement.Domain.Entities;

namespace TJ.ProductManagement.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetById(string id);
        IQueryable<Product> GetAll();
        Task Add(Product entity);
        Task Update(Product entity);
        Task Remove(Product entity);
        Task<long> CountItems();
    }
}
