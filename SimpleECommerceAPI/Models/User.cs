using SimpleECommerceAPI.Models.Enums;

namespace SimpleECommerceAPI.Models
{
    public class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }
        public Guid Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime CreatedAt { get; set; }

        public Cart Cart { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
