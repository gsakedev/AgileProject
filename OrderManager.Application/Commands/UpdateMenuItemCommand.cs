using MediatR;

namespace OrderManager.Application.Commands
{
    public class UpdateMenuItemCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
