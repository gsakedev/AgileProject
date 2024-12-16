using MediatR;
using OrderManager.Application.Commands;
using OrderManager.Application.Mappers;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Factories;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Handlers
{
    public class UpdateMenuItemCommandHandler : IRequestHandler<UpdateMenuItemCommand>
    {
    private readonly IRepository<MenuItem> _menuItemRepository;
    private readonly IMenuItemFactory _menuItemFactory;
    public UpdateMenuItemCommandHandler(IRepository<MenuItem> menuItemRepository, IMenuItemFactory menuItemFactory)
    {
        _menuItemRepository = menuItemRepository;
        _menuItemFactory = menuItemFactory;
    }
        public async Task Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var existingMenuItem = await _menuItemRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingMenuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {request.Id} not found.");
            }

            var dto = MenuItemMapper.ToDTO(existingMenuItem);
            _menuItemFactory.Update(existingMenuItem, dto);
            await _menuItemRepository.SaveChangesAsync(cancellationToken);
        }
    }
}