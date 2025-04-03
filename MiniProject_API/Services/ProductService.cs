using AutoMapper;
using Azure;
using MiniProject_API.Models;
using MiniProject_API.Models.DTO;
using MiniProject_API.Repository;
using MiniProject_API.Repository.IRepository;
using MiniProject_API.Services.IServices;
using System.Net;

namespace MiniProject_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _response = new APIResponse();
            _categoryRepository = categoryRepository;
        }

        public async Task<APIResponse> CreateProductAsync(ProductCreateDTO productCreateDTO)
        {
            try
            {
                if (productCreateDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Invalid data." };
                    return _response;
                }

                var categoryExists = await _categoryRepository.GetAsync(c => c.Id == productCreateDTO.CategoryId);
                if (categoryExists == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Category ID is invalid." };
                    return _response;
                }

                Product cate = _mapper.Map<Product>(productCreateDTO);
                await _productRepository.CreateAsync(cate);
                _response.Result = _mapper.Map<ProductDTO>(cate);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
        }

        public async Task<APIResponse> DeleteProductAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Invalid Data" };
                    return _response;
                }
                var cateToDelete = await _productRepository.GetAsync(x => x.Id == id);
                if (cateToDelete == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Result = "Product not found.";
                    return _response;
                }
                await _productRepository.RemoveAsync(cateToDelete);
                _response.IsSuccess = true;
                _response.Result = $"Delete Product {cateToDelete.Title} Successfullly!!!";
                _response.StatusCode = HttpStatusCode.OK;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;

            }
        }

        public async Task<APIResponse> GetAllProductsAsync()
        {
            try
            {
                var listCate = await _productRepository.GetAllAsync();
                _response.Result = _mapper.Map<List<ProductDTO>>(listCate);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
        }

        public async Task<APIResponse> GetProductByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.Errors = new List<string> { "Invalid ID" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return _response;
                }
                var cate = await _productRepository.GetAsync(x => x.Id == id);
                if (cate == null)
                {
                    _response.Errors = new List<string> { "NOT FOUND Product!!!" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return _response;
                }
                var cateDTO = _mapper.Map<ProductDTO>(cate);
                _response.Result = cateDTO;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
        }

        public async Task<APIResponse> UpdateProductAsync(int id, ProductUpdateDTO productDTO)
        {
            try
            {
                if (productDTO == null || id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Invalid Data" };
                    return _response;
                }

                var existingProduct = await _productRepository.GetAsync(x => x.Id == id);
                if (existingProduct == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string> { "Product not found." };
                    return _response;
                }

                var categoryExists = await _categoryRepository.GetAsync(c => c.Id == productDTO.CategoryId);
                if (categoryExists == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Category ID is invalid." };
                    return _response;
                }

                _mapper.Map(productDTO, existingProduct);
                await _productRepository.UpdateAsync(existingProduct);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = $"Update Product {productDTO.Title} Successfully!!!";
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
        }

    }

}
