using MiniProject_API.Data;
using MiniProject_API.Models;
using MiniProject_API.Repository.IRepository;

namespace MiniProject_API.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Product> UpdateAsync(Product entity)
        {
            //entity.UpdatedDate = DateTime.Now;
            _db.Products.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
