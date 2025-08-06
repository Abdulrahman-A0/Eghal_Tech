using System.ComponentModel.DataAnnotations;

namespace EghalTech.ViewModels
{
    public class ReviewFormViewModel
    {
        public int ProductId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
