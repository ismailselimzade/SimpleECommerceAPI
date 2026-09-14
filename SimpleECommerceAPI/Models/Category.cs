namespace SimpleECommerceAPI.Models
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
