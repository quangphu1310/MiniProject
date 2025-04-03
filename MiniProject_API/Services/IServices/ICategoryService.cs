using MiniProject_API.Models;
using MiniProject_API.Models.DTO;

namespace MiniProject_API.Services.IServices
{
    public interface ICategoryService
    {
        Task<APIResponse> GetAllCategoriesAsync();
        Task<APIResponse> GetCategoryByIdAsync(int id);
        Task<APIResponse> CreateCategoryAsync(CategoryCreateDTO categoryDTO);
        Task<APIResponse> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryDTO);
        Task<APIResponse> DeleteCategoryAsync(int id);
    }
}
