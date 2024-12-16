using OrderManager.Domain.DTOs;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Queries;
using System.Threading.Tasks;

namespace OrderManager.Domain.Interfaces
{
    public interface IMenuItemService
    {
        Task<MenuItemDTO> GetMenuItemByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PagedResult<MenuItemDTO>> GetAllMenuItemsAsync(MenuItemQueryParameters parameters, CancellationToken cancellationToken);
        Task<Guid> CreateMenuItemAsync(MenuItemDTO menuItemDTO, CancellationToken cancellationToken);
        Task UpdateMenuItemAsync(MenuItemDTO menuItemDTO, CancellationToken cancellationToken);
        Task DeleteMenuItemAsync(Guid id, CancellationToken cancellationToken);

    }
}
