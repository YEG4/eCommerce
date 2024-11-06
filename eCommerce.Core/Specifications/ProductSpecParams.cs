namespace eCommerce.Core.Specifications
{
    public class ProductSpecParams
    {
        private int _pageSize = 5;

        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public int PageSize { get => _pageSize; set => _pageSize = value > 10 ? 10 : value; }
        public int PageIndex { get; set; } = 1;

    }
}