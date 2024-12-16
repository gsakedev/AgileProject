using MediatR;
using OrderManager.Application.Helpers;
using OrderManager.Domain.Enums;
using System.Text.Json.Serialization;


namespace OrderManager.Application.Commands
{
    public class PlaceOrderCommand : IRequest<Guid>
    {
        public List<OrderItemCommand> Items { get; set; }

        [JsonConverter(typeof(DeliveryOptionJsonConverter))]
        public DeliveryOption DeliveryOption { get; set; }
        public string? SpecialInstructions { get; set; }
    }
    public class OrderItemCommand
    {
        public Guid MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
