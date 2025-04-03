using MiniProject_API.Models;
using MiniProject_API.Models.DTO;

namespace MiniProject_API.Services.IServices
{
    public interface IProductService
    {
        Task<APIResponse> GetAllProductsAsync();
        Task<APIResponse> GetProductByIdAsync(int id);
        Task<APIResponse> CreateProductAsync(ProductCreateDTO productCreateDTO);
        Task<APIResponse> UpdateProductAsync(int id, ProductUpdateDTO productUpdateDTO);
        Task<APIResponse> DeleteProductAsync(int id);
    }
}
