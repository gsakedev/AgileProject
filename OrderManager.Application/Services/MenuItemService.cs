using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManager.Application.Commands;
using OrderManager.Application.Handlers;
using OrderManager.Application.Mappers;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Queries;

namespace OrderManager.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IRepository<MenuItem> _menuItemRepository;
        private readonly IMediator _mediator;

        public MenuItemService(IRepository<MenuItem> menuItemRepository, IMediator mediator)
        {
            _menuItemRepository = menuItemRepository;
            _mediator = mediator;

        }
        public async Task<Guid> CreateMenuItemAsync(MenuItemDTO menuItemDTO, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(new CreateMenuItemCommand 
            {
                 Name = menuItemDTO.Name,
                 Description = menuItemDTO.Description,
                 IsAvailable = menuItemDTO.IsAvailable,
                 Price = menuItemDTO.Price,
            }, cancellationToken);

            return id;
        }

        public async Task DeleteMenuItemAsync(Guid id, CancellationToken cancellationToken)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id, cancellationToken);
            if (menuItem == null)
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");

            await _menuItemRepository.SoftDelete(menuItem);
            await _menuItemRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<MenuItemDTO>> GetAllMenuItemsAsync(MenuItemQueryParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMenuItemsQuery { Parameters = parameters }, cancellationToken);
            return result;
        }

        public async Task<MenuItemDTO> GetMenuItemByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id, cancellationToken);
            if (menuItem == null)
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");

            return MenuItemMapper.ToDTO(menuItem);
        }

        public async Task UpdateMenuItemAsync(MenuItemDTO menuItemDTO, CancellationToken cancellationToken)
        {

            if (menuItemDTO.Id == null)
                throw new KeyNotFoundException($"Menu item with ID {menuItemDTO.Id} not found.");
            await _mediator.Send(new UpdateMenuItemCommand
            {
                Id = (Guid)menuItemDTO.Id,
                Name = menuItemDTO.Name,
                Description = menuItemDTO.Description,
                IsAvailable = menuItemDTO.IsAvailable,
                Price = menuItemDTO.Price,
            }, cancellationToken);
        }
    }
}
