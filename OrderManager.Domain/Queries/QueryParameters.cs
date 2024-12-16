using Microsoft.AspNetCore.Mvc;
using OrderManager.Domain.Enums;

namespace OrderManager.Domain.Queries
{
    public class QueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "OrderDate";
        public bool IsDescending { get; set; } = false;
    }

    public class OrderQueryParameters : QueryParameters
    {
        public Guid? CustomerId { get; set; }

        [FromQuery(Name = "DeliveryOption")]
        public DeliveryOption? DeliveryOption { get; set; }

        [FromQuery(Name = "State")]
        public OrderState? State { get; set; }
        public bool? IsCompleted { get; set; }
        public decimal? Price { get; set; }
    }

    public class MenuItemQueryParameters : QueryParameters
    {
        public string? Name { get; set; } 
        public decimal? MinPrice { get; set; } 
        public decimal? MaxPrice { get; set; } 
        public bool? IsAvailable { get; set; }
        public string? SortBy { get; set; } = "Name";
        public string? SearchTerm { get; set; }
    }
    public class DeliveryStaffQueryParameters : QueryParameters
    {
        public bool? isAvailable { get; set; }
        public string? id { get; set; }
        public string? location { get; set; }
        public string? SortBy { get; set; } = "isAvailable";

    }
}
