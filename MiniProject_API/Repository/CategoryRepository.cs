using MiniProject_API.Data;
using MiniProject_API.Models;
using MiniProject_API.Repository.IRepository;

namespace MiniProject_API.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Category> UpdateAsync(Category entity)
        {
            //entity.UpdatedDate = DateTime.Now;
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
