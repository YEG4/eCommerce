using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eCommerce.API.DTOs;
using eCommerce.API.Helpers;
using eCommerce.Core.Entities;
using eCommerce.Core.Repositories;
using eCommerce.Core.Specifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    public class ProductsController : APIBaseController
    {
        public IGenericRepository<Product> _productRepo { get; }
        private readonly IMapper _mapper;
        public IGenericRepository<ProductType> _typeRepo { get; }
        public IGenericRepository<ProductBrand> _brandRepo { get; }
        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductType> typeRepo, IGenericRepository<ProductBrand> brandRepo, IMapper mapper)
        {
            _typeRepo = typeRepo;
            _mapper = mapper;
            _productRepo = productRepo;
            _brandRepo = brandRepo;
        }

        [HttpGet]
        /* IReadOnlyList is preferred to be used here because i'm only retrieving data from database and returning it and not doing any iteration over the list.
        If i want to iterate over the list, then IEnumerable<> is better. */
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetAll([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(specParams);
            var products = await _productRepo.GetAllWithSpecificationsAsync(spec);
            var productsMapped = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
            var countSpec = new ProductWithFiltrationForCountAsync(specParams);
            var count = await _productRepo.GetCountWithSpecification(countSpec);
            return Ok(new Pagination<ProductToReturnDTO>(specParams.PageIndex, specParams.PageSize, count, productsMapped));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _productRepo.GetEntityWithSpecificationAsync(spec);
            var productsMapped = _mapper.Map<Product, ProductToReturnDTO>(product);
            return Ok(productsMapped);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var productTypes = await _typeRepo.GetAllAsync();
            return Ok(productTypes);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var brands = await _brandRepo.GetAllAsync();
            return Ok(brands);
        }
    }
}