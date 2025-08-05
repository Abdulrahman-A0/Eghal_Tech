using EghalTech.Models;

namespace EghalTech.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public ProductCardViewModel FeaturedProducts { get; set; }
    }
}
