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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepo = productRepository;
            _response = new APIResponse();
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            try
            {
                var listCate = await _productRepo.GetAllAsync();
                _response.Result = _mapper.Map<List<ProductDTO>>(listCate);
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
        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<APIResponse>> GetProductById(int id)
        {
            try
            {
                if (id == 0) {
                    _response.Errors = new List<string> {"Invalid ID"};
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest   (_response);
                }
                var cate = await _productRepo.GetAsync(x => x.Id == id);
                if (cate == null) {
                    _response.Errors = new List<string> { "NOT FOUND Product!!!" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                var cateDTO = _mapper.Map<ProductDTO>(cate);
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
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductCreateDTO ProductDTO)
        {
            try
            {
                if (ProductDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                Product cate = _mapper.Map<Product>(ProductDTO);
                await _productRepo.CreateAsync(cate);
                _response.Result = _mapper.Map<ProductDTO>(cate);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProductById", new { Id = cate.Id }, _response);
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }
        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int id, [FromBody] ProductUpdateDTO ProductDTO)
        {
            try
            {
                if (ProductDTO == null || id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Invalid Data" };
                    return BadRequest();
                }
                var existingProduct = await _productRepo.GetAsync(x => x.Id == id);
                if (existingProduct == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string> { "Product not found." };
                    return NotFound(_response);
                }

                var duplicateProduct = await _productRepo.GetAsync(c => c.Title.ToLower() == ProductDTO.Title.ToLower() && c.Id != id);
                if (duplicateProduct != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Conflict;
                    _response.Errors = new List<string> { "Product name already exists." };
                    return Conflict(_response);
                }
                _mapper.Map(ProductDTO, existingProduct);
                await _productRepo.UpdateAsync(existingProduct);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.Result = $"Update Product {ProductDTO.Title} Successfully!!!";
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
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
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
                var cateToDelete = await _productRepo.GetAsync(x => x.Id == id);
                if (cateToDelete == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Result = "Product not found.";
                    return NotFound(_response);
                }
                await _productRepo.RemoveAsync(cateToDelete);
                _response.IsSuccess = true;
                _response.Result = $"Delete Product {cateToDelete.Title} Successfullly!!!";
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
