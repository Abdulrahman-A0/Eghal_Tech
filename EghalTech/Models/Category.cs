using System.ComponentModel.DataAnnotations;

namespace EghalTech.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter A Name for the category")]
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; } = [];
    }
}
