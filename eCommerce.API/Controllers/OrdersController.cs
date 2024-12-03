using System.Security.Claims;
using AutoMapper;
using eCommerce.API.DTOs;
using eCommerce.API.Errors;
using eCommerce.Core;
using eCommerce.Core.Entities.Order_Aggregation;
using eCommerce.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{

    public class OrdersController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IOrderService orderService, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._orderService = orderService;
            this._mapper = mapper;
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var shippingAddress = _mapper.Map<AddressDTO, Address>(orderDTO.ShippingAddress);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.CreateOrderAsync(email, orderDTO.BasketId, orderDTO.DeliveryMethodId, shippingAddress);
            if (order is null) return BadRequest(new ApiErrorResponse(400, "Couldn't create the order."));
            var orderMapped = _mapper.Map<Order, OrderToReturnDTO>(order);
            return Ok(orderMapped);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForSpecificUser(userEmail!);
            if (orders is null) return NotFound(new ApiErrorResponse(404, "There are no orders yet"));
            var orderMapped = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders);
            return Ok(orderMapped);
        }
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // it's required or it will return 404 not found. need to search why.
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForSpecificUser(int id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForSpecificUser(userEmail!, id);
            if (order is null) return NotFound(new ApiErrorResponse(404, $"There's no order with the ID:{id}."));
            var orderMapped = _mapper.Map<Order, OrderToReturnDTO>(order);
            return Ok(orderMapped);
        }
    }
}
