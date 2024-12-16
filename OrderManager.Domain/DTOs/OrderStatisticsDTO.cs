namespace OrderManager.Domain.DTOs
{
    public class OrderStatisticsDTO : StatisticsDTO
    {
        public int TotalOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public Dictionary<string, int> OrdersByState { get; set; }
        public double AverageDeliveryTime { get; set; }
    }

    public class DeliveryStaffStatisticsDTO : StatisticsDTO
    {
        public string DeliveryStaffName { get; set; }
        public int TotalDeliveries { get; set; }
        public double AverageDeliveryTime { get; set; }
    }
}
