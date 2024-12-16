namespace OrderManager.Domain.DTOs
{
    public class DeliveryStaffDTO
    {
        public string Id { get; set; }
        public bool IsAvailable { get; set; }
        public string? CurrentLocation { get; set; }
        public Guid? CurrentOrderId { get; set; }
    }
}
