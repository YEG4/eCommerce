namespace eCommerce.API.DTOs
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PrictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}