using AutoMapper;
using eCommerce.API.DTOs;
using eCommerce.API.Errors;
using eCommerce.Core.Entities;
using eCommerce.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            this._paymentService = paymentService;
            this._mapper = mapper;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket =  await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiErrorResponse(400, $"There is no basket with this basket ID:{basketId}"));
            var basketDTO = _mapper.Map<CustomerBasket, CustomerBasketDTO>(basket);
            return Ok(basketDTO);
        }
    }
}
