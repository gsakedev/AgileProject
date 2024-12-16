using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManager.Application.Commands;
using OrderManager.Domain.Constants;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Queries;

namespace OrderManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Places a new order.
        /// </summary>
        /// <param name="dto">The order details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The unique identifier of the created order.</returns>
        /// 
        [Authorize(Policy = Policies.CustomerPolicy)]
        [HttpPost("placeOrder")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDTO dto, CancellationToken cancellationToken)
        {
            var orderId = await _orderService.PlaceOrderAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(PlaceOrder), new { id = orderId }, orderId);
        }

        /// <summary>
        /// Moves the order to the next phase.
        /// </summary>
        /// <param name="orderId">The ID of the order to move.</param>        
        /// <response code="401">Unauthorized User</response>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        ///
        [Authorize(Policy = Policies.RestaurantStaffPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("{orderId}/move")]
        public async Task<IActionResult> MoveOrderToNextPhase(Guid orderId, CancellationToken cancellationToken)
        {
            await _orderService.MoverOrderToNext(orderId, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        /// 
        [Authorize(Policy = Policies.RestaurantStaffPolicy)]
        [HttpPost("{orderId}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelOrder(Guid orderId, CancellationToken cancellationToken)
        {
            await _orderService.CancelOred(orderId, cancellationToken);
            return Ok(new { Message = $"Order {orderId} has been canceled successfully." });
        }

        /// <summary>
        /// Updates the delivery status of an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to update.</param>
        /// <param name="delivered">True if the order was delivered successfully; false if delivery failed.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the update.</returns>
        /// 
        [Authorize(Policy = Policies.DeliveryStaffPolicy)]
        [HttpPut("{orderId}/deliveredStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateDeliveredStatus(Guid orderId, [FromQuery] bool delivered,
            CancellationToken cancellationToken)
        {
            await _orderService.UpdateDeliveryStatus(orderId, delivered, cancellationToken);

            return Ok(new { Message = $"Order {orderId} has been updated to {(delivered ? "Delivered" : "UnableToDeliver")}." });
        }

        /// <summary>
        /// Retrieves a paginated list of orders with optional filtering.
        /// </summary>
        /// <param name="parameters">Filtering, sorting, and paging options.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A paginated list of orders.</returns>
        /// 
        [Authorize]
        [HttpGet("getOrders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<Order>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrders([FromQuery] OrderQueryParameters parameters, CancellationToken cancellationToken)
        {
            return Ok(await _orderService.GetOrders(parameters, cancellationToken));
        }

        /// <summary>
        /// Assigns a delivery staff member to an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to be assigned.</param>
        /// <param name="deliveryStaffId">The delivery Staff staff ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="NoContentResult"/> if the assignment is successful.</returns>
        /// <response code="202">The assignment was successful.</response>
        /// <response code="400">The request is invalid (e.g., missing order ID or delivery staff ID).</response>
        /// <response code="404">The order or delivery staff was not found.</response>
        /// <response code="500">An internal server error occurred.</response>
        [HttpPost("{orderId}/assignDelivery/{deliveryStaffId}")]
        [Authorize(Policy = Policies.RestaurantStaffPolicy)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignDelivery(Guid orderId, Guid  deliveryStaffId, CancellationToken cancellationToken)
        {
            await _orderService.AssignDelivery(orderId, deliveryStaffId, cancellationToken);
            return Accepted();
        }

        /// <summary>
        /// Retrieves a list of available delivery staff with filtering, sorting, and pagination.
        /// </summary>
        /// <param name="parameters">Query parameters for filtering, sorting, and pagination.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A paginated list of available delivery staff.</returns>
        [HttpGet("deliveryStaff/available")]
        [Authorize(Policy = Policies.RestaurantStaffPolicy)]
        public async Task<IActionResult> GetAvailableDeliveryStaff([FromQuery] DeliveryStaffQueryParameters parameters, CancellationToken cancellationToken)
        {
            return Ok(await _orderService.GetAvailableDeliveryStaff(parameters, cancellationToken));
        }
    }
}
