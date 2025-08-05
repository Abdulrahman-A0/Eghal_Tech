using EghalTech.Models;
using X.PagedList;

namespace EghalTech.ViewModels
{
    public class ProductCardViewModel
    {
        public IPagedList<Product> Products { get; set; }
        public List<int> WishListProductIds { get; set; }
    }
}
