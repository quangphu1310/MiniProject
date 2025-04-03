using Asp.Versioning;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject_API.Models;
using MiniProject_API.Models.DTO;
using MiniProject_API.Repository.IRepository;
using MiniProject_API.Services;
using MiniProject_API.Services.IServices;
using System.Net;

namespace MiniProject_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productService.GetAllProductsAsync();
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductCreateDTO ProductDTO)
        {
            var response = await _productService.CreateProductAsync(ProductDTO);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO ProductDTO)
        {
            var response = await _productService.UpdateProductAsync(id, ProductDTO);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpDelete]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
