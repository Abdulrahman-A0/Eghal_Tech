namespace EghalTech.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = [];

        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
    }
    public class OrderItemViewModel
    {
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
    }
}
