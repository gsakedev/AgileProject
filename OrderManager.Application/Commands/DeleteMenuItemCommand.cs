using MediatR;

namespace OrderManager.Application.Commands
{
    public class DeleteMenuItemCommand : IRequest
    {
        public Guid Id { get; set; }

    }
}
