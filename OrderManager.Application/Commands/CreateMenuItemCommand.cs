using MediatR;

namespace OrderManager.Application.Commands
{
    public class CreateMenuItemCommand : IRequest<Guid>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
