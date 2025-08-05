using EghalTech.Data;
using EghalTech.Models;

namespace EghalTech.Repository
{
    public class WishlistRepository : Repository<WishList>, IWishlistRepository
    {
        public WishlistRepository(AppDbContext _context) : base(_context)
        {
        }
        public bool Exists(string userId, int productId)
        {
            return context.WishListItems
                .Any(w => w.Wishlist.UserId == userId && w.ProductId == productId);
        }
    }
}
