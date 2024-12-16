namespace OrderManager.Domain.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string DeliveryOption { get; set; } 
        public string State { get; set; }
        public string? SpecialInstructions { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal OrderTotalPrice { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
