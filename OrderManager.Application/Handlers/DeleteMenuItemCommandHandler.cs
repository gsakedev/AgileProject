using MediatR;
using OrderManager.Application.Commands;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Handlers
{
    public class DeleteMenuItemCommandHandler : IRequestHandler<DeleteMenuItemCommand>
    {
        private readonly IRepository<MenuItem> _menuItemRepository;

        public DeleteMenuItemCommandHandler(IRepository<MenuItem> menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }
        public async Task Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(request.Id, cancellationToken);
            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {request.Id} not found.");
            }

            menuItem.IsDeleted = true;
            await _menuItemRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
