using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManager.Domain.Constants;
using OrderManager.Domain.Interfaces;

namespace OrderManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Policies.AdminPolicy)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        //TODO: ADD VALIDATIONS DOESNT WORK PROPERLY
        /// <summary>
        /// Get the average delivery time for delivered orders within a specified date range.
        /// </summary>
        [HttpGet("average-delivery-time")]
        public async Task<IActionResult> GetAverageDeliveryTime(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            var result = await _adminService.GetAverageDeliveryTimeAsync(startDate, endDate, cancellationToken);
            return Ok(new { AverageDeliveryTime = result });
        }

        ///// <summary>
        ///// Get the count of orders delivered by a specific delivery staff within a date range.
        ///// </summary>
        //[HttpGet("delivered-orders-count")]
        //public async Task<IActionResult> GetDeliveredOrdersCount(Guid staffId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        //{
        //    var count = await _adminService.GetDeliveredOrdersCountByStaffAsync(staffId, startDate, endDate, cancellationToken);
        //    return Ok(new { StaffId = staffId, DeliveredOrdersCount = count });
        //}
    }
}
