using OrderManager.Domain.Entities;

namespace OrderManager.Domain.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<List<MenuItem>> GetAvailableMenuItemsAsync(IEnumerable<Guid> menuItemIds);
    }
}
