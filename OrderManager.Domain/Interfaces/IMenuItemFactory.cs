using OrderManager.Domain.DTOs;
using OrderManager.Domain.Entities;

namespace OrderManager.Domain.Interfaces
{
    public interface IMenuItemFactory
    {
        MenuItem Create(string name, string description, decimal price, bool isAvailable, Guid? id);
        void Update(MenuItem menuItem, MenuItemDTO dto);

    }
}
