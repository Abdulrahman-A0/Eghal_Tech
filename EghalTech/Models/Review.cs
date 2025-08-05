using System.ComponentModel.DataAnnotations;

namespace EghalTech.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Range(1, 5, ErrorMessage = "Rating should be from 1 to 5")]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
