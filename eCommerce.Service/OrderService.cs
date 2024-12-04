using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Core;
using eCommerce.Core.Entities;
using eCommerce.Core.Entities.Order_Aggregation;
using eCommerce.Core.Repositories;
using eCommerce.Core.Services;
using eCommerce.Core.Specifications.Order_Specification;
using NetTopologySuite.Index.HPRtree;

namespace eCommerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            this._basketRepo = basketRepo;
            this._unitOfWork = unitOfWork;
            this._paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string email, string basketId, int methodDeliveryId, Address shippingAddress)
        {
            // Get Basket from basketId to create a list of orderItems where its prices are fetched from database
            var basket = await _basketRepo.GetBasketAsync(basketId);
            var orderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productOrdered = new ProductOrdered(product.Id, product.Name, product.PictureUrl);
                    orderItems.Add(new OrderItem(productOrdered, product.Price, item.Quantity));
                }
            }
            // Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Quantity * item.Price);
            // Get DeliveryMethod Cost 
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(methodDeliveryId);

            var orderSpecification = new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId);
            var exOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationAsync(orderSpecification);
            if(exOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(exOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }
            // Create Order 
            var order = new Order(email, shippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);

            // Save to Database
            await _unitOfWork.Repository<Order>().AddAsync(order);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                return null;
            }
            return order;

        }

        public async Task<Order?> GetOrderByIdForSpecificUser(string email, int orderId)
        {
            var orderSpecifications = new OrderSpecification(email, orderId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationAsync(orderSpecifications);
            return order;
        }

        public async Task<IReadOnlyList<Order?>> GetOrdersForSpecificUser(string email)
        {
            var orderSpecifications = new OrderSpecification(email);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecificationsAsync(orderSpecifications);
            return orders;
        }
    }
}
