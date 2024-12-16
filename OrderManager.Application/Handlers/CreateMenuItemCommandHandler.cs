using MediatR;
using OrderManager.Application.Commands;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Handlers
{
    public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, Guid>
    {
        private readonly IRepository<MenuItem> _menuItemRepository;
        private readonly IMenuItemFactory _menuItemFactory;

        public CreateMenuItemCommandHandler(IRepository<MenuItem> menuItemRepository, IMenuItemFactory menuItemFactory)
        {
            _menuItemRepository = menuItemRepository;
            _menuItemFactory = menuItemFactory;
        }
        public async Task<Guid> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = _menuItemFactory.Create(request.Name, request.Description, request.Price, request.IsAvailable, request.Id);

            await _menuItemRepository.AddAsync(menuItem, cancellationToken);
            await _menuItemRepository.SaveChangesAsync(cancellationToken);

            return menuItem.Id;
        }
    }
}
