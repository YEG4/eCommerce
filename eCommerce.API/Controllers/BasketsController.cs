using eCommerce.API.Errors;
using eCommerce.Core.Entities;
using eCommerce.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    public class BasketsController : APIBaseController
    {
        private readonly IBasketRepository _basketRepo;

        public BasketsController(IBasketRepository basketRepo)
        {
            this._basketRepo = basketRepo;
        }

        [HttpGet("{basketId}")]
        public async Task<ActionResult<CustomerBasket?>> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket is null) return Ok(new CustomerBasket(basketId));
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket?>> CreateOrUpdateBasketAsync(CustomerBasket? basket)
        {
            var createdOrUpdatedBasket = await _basketRepo.CreateOrUpdateBasketAsync(basket);
            if (basket is null) return BadRequest(new ApiErrorResponse(400));
            return Ok(createdOrUpdatedBasket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string basketId) 
        { 
            return await _basketRepo.DeleteBasketAsync(basketId);
        }
    }
}
