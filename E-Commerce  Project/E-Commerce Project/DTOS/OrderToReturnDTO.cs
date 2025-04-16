

using Core.Entities.Order;

namespace E_Commerce_Project.DTOS
{
    public class OrderToReturnDTO
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public string Status { get; set; } 
        public ShippingAddress ShippingAddress { get; set; }
        //public int DeliveryMethodId { get; set; }  //FK
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>();
        public decimal SupTotal { get; set; }

        public decimal Total { get; set; }
        public string PaymentintentId { get; set; } = string.Empty;
    }
}
