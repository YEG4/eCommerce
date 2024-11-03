using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Core.Entities;
using eCommerce.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    public class ProductController : APIBaseController
    {
        public IGenericRepository<Product> _productRepo { get; }
        public ProductController(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _productRepo.GetAllAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            return Ok(product);
        }
    }
}