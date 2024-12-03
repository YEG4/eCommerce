using eCommerce.Core.Entities.Order_Aggregation;

namespace eCommerce.API.DTOs
{
    public class OrderToReturnDTO
    {
        public  int Id { get; set; }
        public string BuyerEmail { get; set; }
        public string OrderDate { get; set; }
        public string status { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; }  = string.Empty;
    }
}
