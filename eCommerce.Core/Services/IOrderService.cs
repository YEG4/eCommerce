using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Core.Entities.Order_Aggregation;

namespace eCommerce.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string email, string basketId, int methodDeliveryId, Address shippingAddress);
        Task<IReadOnlyList<Order?>> GetOrdersForSpecificUser(string email);

        Task<Order?> GetOrderByIdForSpecificUser(string email, int orderId);
    }
}
