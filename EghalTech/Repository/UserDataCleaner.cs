using EghalTech.Models;

namespace EghalTech.Repository
{
    public class UserDataCleaner : IUserDataCleaner
    {
        private readonly IRepository<WishList> wishlistRepo;
        private readonly IRepository<WishListItem> wishItemsRepo;
        private readonly IRepository<Cart> cartRepo;
        private readonly IRepository<CartItem> cartItemsRepo;
        private readonly IRepository<Review> reviewRepo;

        public UserDataCleaner(
            IRepository<WishList> _wishlistRepo,
            IRepository<WishListItem> _wishItemsRepo,
            IRepository<Cart> _cartRepo,
            IRepository<CartItem> _cartItemsRepo,
            IRepository<Review> _reviewRepo
            )
        {
            wishlistRepo = _wishlistRepo;
            wishItemsRepo = _wishItemsRepo;
            cartRepo = _cartRepo;
            cartItemsRepo = _cartItemsRepo;
            reviewRepo = _reviewRepo;
        }

        public void DeleteUserDataAsync(User user)
        {
            if (user.WishList?.WishlistItems != null)
            {
                foreach (var item in user.WishList.WishlistItems)
                    wishItemsRepo.Delete(item.Id);

                wishlistRepo.Delete(user.WishList.Id);
            }

            if (user.Cart?.CartItems != null)
            {
                foreach (var item in user.Cart.CartItems)
                    cartItemsRepo.Delete(item.Id);

                cartRepo.Delete(user.Cart.Id);
            }

            if (user.Reviews?.Count > 0)
            {
                foreach (var rev in user.Reviews)
                    reviewRepo.Delete(rev.Id);
            }


        }
    }
}
