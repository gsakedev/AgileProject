using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManager.Domain.Constants;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }
        /// Gets a menu item by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(MenuItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMenuItem(Guid id, CancellationToken cancellationToken)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id, cancellationToken);
            return Ok(menuItem);
        }
        /// <summary>
        /// 
        /// Gets all menu items.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<MenuItemDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMenuItems([FromQuery] MenuItemQueryParameters parameters, CancellationToken cancellationToken)
        {
            var menuItems = await _menuItemService.GetAllMenuItemsAsync(parameters, cancellationToken);
            return Ok(menuItems);
        }

        /// <summary>
        /// Creates a new menu item.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = Policies.AdminPolicy)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemDTO menuItemDTO, CancellationToken cancellationToken)
        {
            var id = await _menuItemService.CreateMenuItemAsync(menuItemDTO, cancellationToken);
            return CreatedAtAction(nameof(GetMenuItem), new { id }, id);
        }

        /// <summary>
        /// Updates an existing menu item.
        /// </summary>
        /// 
        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policies.AdminPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMenuItem(Guid id, [FromBody] MenuItemDTO menuItemDTO, CancellationToken cancellationToken)
        {
            await _menuItemService.UpdateMenuItemAsync(menuItemDTO, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Deletes a menu item.
        /// </summary>
        /// 
        [Authorize(Policy = Policies.AdminPolicy)]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMenuItem(Guid id, CancellationToken cancellationToken)
        {
            await _menuItemService.DeleteMenuItemAsync(id, cancellationToken);
            return Accepted();
        }
    }
}
