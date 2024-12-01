namespace eCommerce.Core.Entities.Order_Aggregation
{
    public class ProductOrdered
    {
        public ProductOrdered()
        {
            
        }
        public ProductOrdered(int productId, string productName, string prictureUrl)
        {
            Id = productId;
            Name = productName;
            PrictureUrl = prictureUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PrictureUrl { get; set; }
    }
}