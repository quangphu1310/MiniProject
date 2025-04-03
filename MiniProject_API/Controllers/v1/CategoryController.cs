using Asp.Versioning;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject_API.Models;
using MiniProject_API.Models.DTO;
using MiniProject_API.Repository.IRepository;
using MiniProject_API.Services.IServices;
using System.Net;

namespace MiniProject_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryCreateDTO categoryDTO)
        {
            var response = await _categoryService.CreateCategoryAsync(categoryDTO);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO categoryDTO)
        {
            var response = await _categoryService.UpdateCategoryAsync(id, categoryDTO);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
