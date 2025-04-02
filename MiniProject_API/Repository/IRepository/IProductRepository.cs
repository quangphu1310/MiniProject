using MiniProject_API.Models;

namespace MiniProject_API.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> UpdateAsync(Product product);
    }
}
