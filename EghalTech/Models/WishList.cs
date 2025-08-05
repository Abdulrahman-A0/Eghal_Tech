namespace EghalTech.Models
{
    public class WishList
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<WishListItem> WishlistItems { get; set; } = [];
    }
}
