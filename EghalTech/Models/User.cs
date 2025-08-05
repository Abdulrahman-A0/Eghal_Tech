using Microsoft.AspNetCore.Identity;

namespace EghalTech.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual WishList? WishList { get; set; }
        public virtual List<Review>? Reviews { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}
