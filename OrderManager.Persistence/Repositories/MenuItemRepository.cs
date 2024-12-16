using Microsoft.EntityFrameworkCore;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;
using OrderManager.Persistence.Contexts;

namespace OrderManager.Persistence.Repositories
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        public async Task<List<MenuItem>> GetAvailableMenuItemsAsync(IEnumerable<Guid> menuItemIds)
        {
            return await Query()
                .Where(mi => menuItemIds.Contains(mi.Id) && mi.IsAvailable && !mi.IsDeleted)
                .ToListAsync();
        }
    }
}
