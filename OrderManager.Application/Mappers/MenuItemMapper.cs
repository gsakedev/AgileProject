using OrderManager.Domain.DTOs;
using OrderManager.Domain.Entities;

namespace OrderManager.Application.Mappers
{
    public class MenuItemMapper
    {
        public static MenuItemDTO ToDTO(MenuItem menuItem)
        {
            return new MenuItemDTO
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable
            };
        }
        public static void UpdateEntity(MenuItem entity, MenuItemDTO dto)
        {
            entity.Name = dto.Name ?? entity.Name;
            entity.Description = dto.Description ?? entity.Description;
            entity.Price = dto.Price != 0 ? dto.Price : entity.Price;
            entity.IsAvailable = dto.IsAvailable;
        }
    }
}
