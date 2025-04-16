using Core.Entities.Identity;
using Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IOrderService
    {
        public Task<Order?> CreateOrderAsync( string BuyerEmail, string BasketId, int DeliveryMethodId, ShippingAddress ShippingAddress);
        public Task<Order> GetOrderByIdforSpecificUserAsync(string BuyerEmail, int OrderId);
        public Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail);



    }
}
