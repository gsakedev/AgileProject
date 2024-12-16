namespace OrderManager.Domain.DTOs
{
    public class OrderItemDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
