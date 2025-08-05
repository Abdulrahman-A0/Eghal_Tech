namespace EghalTech.Models
{
    public class WishListItem
    {
        public int Id { get; set; }

        public int WishlistID { get; set; }
        public virtual WishList Wishlist { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
