using TJ.ProductManagement.Domain.Models;

namespace TJ.ProductManagement.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAll();
        Task<ProductViewModel> GetById(string id);
        Task Add(ProductInsertModel model);
        Task Update(string id, ProductInsertModel model);
        Task Delete(string id);
    }
}
