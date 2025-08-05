using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EghalTech.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Product Brand")]
        public string Brand { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Remote(action: "ValidatePriceInput", controller: "Product", ErrorMessage = "Value Must be greater than 0")]
        public decimal Price { get; set; }

        [Remote(action: "ValidateStockQtyInput", controller: "Product", ErrorMessage = "Value Must be greater than 0")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Please Upload Product Image")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Enter A description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Choose Product Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual List<Review> Reviews { get; set; }
        public virtual List<CartItem> CartItems { get; set; }
        public virtual List<WishListItem> WishlistItems { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
