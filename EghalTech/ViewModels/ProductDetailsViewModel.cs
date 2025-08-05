namespace EghalTech.ViewModels;
using EghalTech.Models;
public class ProductDetailsViewModel
{
    public Product Product { get; set; }
    public List<Review> Reviews { get; set; }
    public bool IsInWishList { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}