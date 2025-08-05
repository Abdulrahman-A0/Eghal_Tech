using EghalTech.Models;

namespace EghalTech.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
    }
}
