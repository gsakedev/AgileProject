namespace OrderManager.Domain.Entities
{
    public class MenuItem
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique identifier
        public string Name { get; set; } // Name of the menu item
        public string? Description { get; set; } // Description of the item
        public decimal Price { get; set; } // Price per unit
        public bool IsAvailable { get; set; } = true; // Availability flag
        public bool IsDeleted { get; set; } = false; // Soft delete flag
    }
}
