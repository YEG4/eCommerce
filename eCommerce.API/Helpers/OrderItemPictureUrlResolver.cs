using AutoMapper;
using eCommerce.API.DTOs;
using eCommerce.Core.Entities.Order_Aggregation;

namespace eCommerce.API.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PrictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PrictureUrl}";
            return "";
        }
    }
}
