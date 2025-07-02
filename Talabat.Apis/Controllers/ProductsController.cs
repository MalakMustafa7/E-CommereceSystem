using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Talabat.Apis.DTOs;
using Talabat.Apis.Errors;
using Talabat.Apis.Helper;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.Apis.Controllers
{
     
    public class ProductsController : ApiBaseController
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        

        public ProductsController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
             
        }
        [CachedAttribute(300)]
        [HttpGet]
         
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
           var Spec= new ProductWithBrandAndTypeSpecification(Params);
           var products=  await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
           var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
           var CountSpec= new ProductWithFiltirationForCountAsync(Params);
           var Count= await _unitOfWork.Repository<Product>().CountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDTO>(Params.PageSize,Params.PageIndex,mappedProducts,Count));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            if(product is null) return NotFound(new ApiResponse(404));
            var mappedProduct = _mapper.Map<Product, ProductToReturnDTO>(product);
            return Ok(mappedProduct);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
    }
}
