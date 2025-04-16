using AutoMapper;
using Core.Contracts;
using Core.Entities;
using Core.Specifications;
using E_Commerce_Project.DTOS;
using E_Commerce_Project.Errors;
using E_Commerce_Project.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Project.Controllers
{

    public class ProductsController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IMapper mapper , IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize]
        [CachedAttribute(1000)]
        // Get All Products
        public async Task<ActionResult<Paggination<ProductToReturnDTO>>> GetAllProducts([FromQuery]ProductSpecParams Parameters)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(Parameters);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            
            var MappedProducts = _mapper.Map<IReadOnlyList<ProductToReturnDTO>>(products);

            var specCount = new ProductWithFiltrationForCountAsync(Parameters);
            var TotalCount = await _unitOfWork.Repository<Product>().GetCountWithSpec(specCount);

            return Ok(new Paggination<ProductToReturnDTO>(Parameters.PageIndex, Parameters.PageSize, MappedProducts, TotalCount));
        }

        [CachedAttribute(1000)]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDTO), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<Paggination<ProductToReturnDTO>>> GetPrpdutById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            
            if(product == null) return NotFound(new ApiResponse(404));

            var MappedProducts = _mapper.Map<ProductToReturnDTO>(product);

            return Ok(MappedProducts);
        }

        [HttpGet("Brands")]
        [CachedAttribute(1000)]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllProductBrands() {

            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("Types")]
        [CachedAttribute(1000)]

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllProductTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }
    }
}
