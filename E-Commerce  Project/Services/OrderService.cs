using Core.Contracts;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.Order;
using Core.Services;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork , IBasketRepository basketRepository ,IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _paymentService = paymentService;
        }

        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, ShippingAddress ShippingAddress)
        {
            // Get Basket
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            // Get Selected item at basket from product Repi
            var OrderItems = new List<OrderItem>();

            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrderd = new ProductItemOrderd(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrderd, item.Quantity, Product.Price);
                    OrderItems.Add(OrderItem);

                }

            }

            // Calculate Subtotal
            var Subtotal = OrderItems.Sum(item => item.Quantity * item.Price);

            // Get Delivery Method
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            // Create Order

            var spec = new OrderWithPaymentIntentIdSpecification(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreatrOrUpdatePaymentIntent(BasketId);
            }

            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItems, Subtotal, Basket.PaymentIntentId);
            // Add order Local
            await _unitOfWork.Repository<Order>().AddAsync(Order);
            // save Order
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;
            return Order;
        }

        public Task<Order> GetOrderByIdforSpecificUserAsync(string BuyerEmail, int OrderId)
        {

            var spec = new OrderSpecifications(BuyerEmail , OrderId);
            var order = _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail) { 

            var spec = new OrderSpecifications(BuyerEmail);
            var orders =await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
