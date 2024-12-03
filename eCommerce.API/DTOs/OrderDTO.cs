using System.ComponentModel.DataAnnotations;

namespace eCommerce.API.DTOs
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
