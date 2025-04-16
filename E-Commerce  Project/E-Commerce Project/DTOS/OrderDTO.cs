using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Project.DTOS
{
    public class OrderDTO
    {
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]

        public AddressDTO ShippingAddress { get; set; }
    }
}
