using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject_API.Models;
using MiniProject_API.Models.DTO;
using MiniProject_API.Repository.IRepository;
using System.Net;

namespace MiniProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _cateRepo;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _cateRepo = categoryRepository;
            _response = new APIResponse();
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            try
            {
                var listCate = await _cateRepo.GetAllAsync();
                _response.Result = _mapper.Map<List<CategoryDTO>>(listCate);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<APIResponse>> GetCategoryById(int id)
        {
            try
            {
                if (id == 0) {
                    _response.Errors = new List<string> {"Invalid ID"};
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest   (_response);
                }
                var cate = await _cateRepo.GetAsync(x => x.Id == id);
                if (cate == null) {
                    _response.Errors = new List<string> { "NOT FOUND CATEGORY!!!" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                var cateDTO = _mapper.Map<CategoryDTO>(cate);
                _response.Result = cateDTO;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                Category cate = _mapper.Map<Category>(categoryDTO);
                await _cateRepo.CreateAsync(cate);
                _response.Result = _mapper.Map<CategoryDTO>(cate);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetCategoryById", new { Id = cate.Id }, _response);
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateCategory(int id, [FromBody] CategoryUpdateDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null || id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Invalid Data" };
                    return BadRequest();
                }
                var existingCategory = await _cateRepo.GetAsync(x => x.Id == id);
                if (existingCategory == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string> { "Category not found." };
                    return NotFound(_response);
                }

                var duplicateCategory = await _cateRepo.GetAsync(c => c.Name.ToLower() == categoryDTO.Name.ToLower() && c.Id != id);
                if (duplicateCategory != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Conflict;
                    _response.Errors = new List<string> { "Category name already exists." };
                    return Conflict(_response);
                }
                _mapper.Map(categoryDTO, existingCategory);
                await _cateRepo.UpdateAsync(existingCategory);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.Result = $"Update Category {categoryDTO.Name} Successfully!!!";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }
        [HttpDelete]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Invalid Data" };
                    return BadRequest(_response);
                }
                var cateToDelete = await _cateRepo.GetAsync(x => x.Id == id);
                if (cateToDelete == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Result = "Category not found.";
                    return NotFound(_response);
                }
                await _cateRepo.RemoveAsync(cateToDelete);
                _response.IsSuccess = true;
                _response.Result = $"Delete Category {cateToDelete.Name} Successfullly!!!";
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
