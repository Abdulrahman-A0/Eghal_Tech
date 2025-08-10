using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EghalTech.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string Address { get; set; }
        public OrderStatus Status { get; set; }

        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; } = [];

    }
    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }
}
