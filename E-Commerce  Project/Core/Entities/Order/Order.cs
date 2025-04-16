using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Order
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal supTotal, string paymentintentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SupTotal = supTotal;
            PaymentintentId = paymentintentId;
        }

        public string BuyerEmail { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        //public int DeliveryMethodId { get; set; }  //FK
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SupTotal { get; set; }

        public decimal GetTotal()
            => SupTotal + DeliveryMethod.Cost;
        public string PaymentintentId { get; set; }

    }
}
