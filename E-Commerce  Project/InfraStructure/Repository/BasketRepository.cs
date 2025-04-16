using Core.Contracts;
using Core.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InfraStructure.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _redis;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            this._redis = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string id)
            => await _redis.KeyDeleteAsync(id);

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var basket =await _redis.StringGetAsync(id);
            return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket?>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var busketSerialize = JsonSerializer.Serialize(basket);
            var updatedBusket = await _redis.StringSetAsync(basket.Id,busketSerialize,TimeSpan.FromDays(1));
            return updatedBusket  ? await GetBasketAsync(basket.Id) : null;
        }
    }
}
