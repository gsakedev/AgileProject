namespace OrderManager.Domain.DTOs
{ 
    /// <summary>
  /// Represents the data required to place an order.
  /// </summary>
  /// 
    public class PlaceOrderDTO
    {
        /// <summary>
        /// The list of items in the order.
        /// </summary>
        public List<OrderItemDto> Items { get; set; }

        /// <summary>
        /// The delivery option for the order (Pickup or Delivery).
        /// </summary>
        public string DeliveryOption { get; set; }

        /// <summary>
        /// Special instructions for the order.
        /// </summary>
        public string? SpecialInstructions { get; set; }
    }

    /// <summary>
    /// Represents a single item in an order.
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// The unique identifier of the menu item.
        /// </summary>
        public Guid MenuItemId { get; set; }

        /// <summary>
        /// The quantity of the menu item being ordered.
        /// </summary>
        public int Quantity { get; set; }

    }
}
