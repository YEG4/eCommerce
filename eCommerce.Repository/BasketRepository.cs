using System.Text.Json;
using System.Text.Json.Nodes;
using eCommerce.Core.Entities;
using eCommerce.Core.Repositories;
using StackExchange.Redis;

namespace eCommerce.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _db;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket)
        {
            var basketSerialized = JsonSerializer.Serialize(basket);
            var result = await _db.StringSetAsync(basket.Id, basketSerialized, TimeSpan.FromDays(1));
            if (!result) return null;
            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var result = await _db.KeyDeleteAsync(basketId);
            return result;
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _db.StringGetAsync(basketId);
            if (basket.IsNull) return null;
            var basketDeserialized = JsonSerializer.Deserialize<CustomerBasket>(basket);
            return basketDeserialized;
        }
    }
}
