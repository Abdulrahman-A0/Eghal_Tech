using EghalTech.Models;

namespace EghalTech.Repository
{
    public interface IWishlistRepository : IRepository<WishList>
    {
        bool Exists(string userId, int productId);
    }
}
