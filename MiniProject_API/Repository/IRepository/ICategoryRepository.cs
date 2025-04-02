using MiniProject_API.Models;

namespace MiniProject_API.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> UpdateAsync(Category category);
    }
}
