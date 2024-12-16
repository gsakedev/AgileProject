using OrderManager.Domain.DTOs;
using OrderManager.Domain.Entities;

namespace OrderManager.Application.Mappers
{
    public static class OrderMapper
    {
        public static OrderDTO ToOrderDTO(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                DeliveryOption = order.DeliveryOption.ToString(), 
                State = order.State.ToString(), 
                SpecialInstructions = order.SpecialInstructions,
                OrderDate = order.OrderDate,
                CompletedDate = order.CompletedDate,
                OrderTotalPrice = order.OrderTotalPrice,
                Items = order.Items.Select(ToOrderItemDTO).ToList()
            };
        }
        public static OrderItemDTO ToOrderItemDTO(OrderItem item)
        {
            return new OrderItemDTO
            {
                Name = item.MenuItem.Name,
                Price = item.Price,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice
            };
        }
    }
}
