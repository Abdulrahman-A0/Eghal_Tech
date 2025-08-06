using EghalTech.Models;
using X.PagedList;

namespace EghalTech.ViewModels
{
    public class ProductCardViewModel
    {
        public IPagedList<ProductCard> Products { get; set; }
        public List<int> WishListProductIds { get; set; }
    }

    public class ProductCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; }

        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
