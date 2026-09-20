namespace SimpleECommerceAPI.QueryParameters
{
    public class ProductQueryParameters
    {
        private int _page = 1;
        public int Page { get => _page; set => _page = Math.Max(value, 1); }
        private int _pageSize = 20;
        public int PageSize { get => _pageSize; set => _pageSize = Math.Clamp(value, 1, 100); }
        public Guid? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SearchTerm { get; set; }
    }
}
