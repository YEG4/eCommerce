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
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = eCommerce.Core.Entities.Product;

namespace eCommerce.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IBasketRepository basketRepository, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            this._basketRepository = basketRepository;
            this._configuration = configuration;
            this._unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket = await _basketRepository.GetBasketAsync(basketId);
            var shippingCost = 0M;
            if (basket is null) return null;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingCost = deliveryMethod.Cost;
            }
            if (basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (product.Price != item.Price)
                            item.Price = product.Price;
                }
            }

            var subTotal = basket.Items.Sum(item => item.Quantity * item.Price);

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            // Check if paymentIntentId is null, if true then this is the first time the customer will purchase if false it means the customer
            // update the total price of the basket.
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)shippingCost * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card" }
                };
                paymentIntent = await service.CreateAsync(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions() 
                { 
                    Amount = (long)subTotal * 100 + (long)shippingCost * 100,
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);
            }

            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

             await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return basket;
        }
    }
}
