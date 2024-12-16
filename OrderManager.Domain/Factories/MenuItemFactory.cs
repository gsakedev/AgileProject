using OrderManager.Domain.DTOs;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Domain.Factories
{
    public class MenuItemFactory : IMenuItemFactory
    {
        public MenuItem Create(string name, string? description, decimal price, bool isAvailable, Guid? id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("MenuItem name cannot be empty.");

            if (price <= 0)
                throw new ArgumentException("MenuItem price must be greater than zero.");

            return new MenuItem
            {
                Id = id ?? Guid.NewGuid(),
                Name = name,
                Description = description,
                Price = price,
                IsAvailable = isAvailable
            };
        }

        public void Update(MenuItem menuItem, MenuItemDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("MenuItem name cannot be empty.");

            if (dto.Price <= 0)
                throw new ArgumentException("MenuItem price must be greater than zero.");
            menuItem.Name = dto.Name;
            menuItem.Description = dto.Description;
            menuItem.Price = dto.Price;
            menuItem.IsAvailable = dto.IsAvailable;
        }
    }
}
