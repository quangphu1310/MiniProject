using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using MiniProject_API.Models;
using MiniProject_API.Models.DTO;
using MiniProject_API.Repository.IRepository;
using MiniProject_API.Services.IServices;
using System.Net;

namespace MiniProject_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse response;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            response = new APIResponse();
        }
        
        public async Task<APIResponse> GetAllCategoriesAsync()
        {
            try
            {
                var listCate = await _categoryRepository.GetAllAsync();
                response.Result = _mapper.Map<List<CategoryDTO>>(listCate);
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }

        public async Task<APIResponse> GetCategoryByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    response.Errors = new List<string> { "Invalid ID" };
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return response;
                }
                var cate = await _categoryRepository.GetAsync(x => x.Id == id);
                if (cate == null)
                {
                    response.Errors = new List<string> { "NOT FOUND CATEGORY!!!" };
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    return response;
                }
                var cateDTO = _mapper.Map<CategoryDTO>(cate);
                response.Result = cateDTO;
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode=HttpStatusCode.BadRequest;
                response.Errors = new List<string> { ex.Message };
                return response;
            }
        }
        public async Task<APIResponse> CreateCategoryAsync(CategoryCreateDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string> { "Invalid Data" };
                    return response;
                }

                Category cate = _mapper.Map<Category>(categoryDTO);
                await _categoryRepository.CreateAsync(cate);
                response.Result = _mapper.Map<CategoryDTO>(cate);
                response.StatusCode = HttpStatusCode.Created;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }

        public async Task<APIResponse> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null || id <= 0)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string> { "Invalid Data" };
                    return response;
                }

                var existingCategory = await _categoryRepository.GetAsync(x => x.Id == id);
                if (existingCategory == null)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Errors = new List<string> { "Category not found." };
                    return response;
                }

                _mapper.Map(categoryDTO, existingCategory);
                await _categoryRepository.UpdateAsync(existingCategory);

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Result = $"Updated Category {categoryDTO.Name} Successfully!!!";
                return response;
            }
            catch (Exception ex) { 
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = new List<string> { ex.Message };
                return response;
            }
        }

        public async Task<APIResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string> { "Invalid Data" };
                    return response;
                }
                var cateToDelete = await _categoryRepository.GetAsync(x => x.Id == id);
                if (cateToDelete == null)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Result = "Category not found.";
                    return response;
                }
                await _categoryRepository.RemoveAsync(cateToDelete);
                response.IsSuccess = true;
                response.Result = $"Delete Category {cateToDelete.Name} Successfullly!!!";
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
                return response;
            }
        }
    }
}
