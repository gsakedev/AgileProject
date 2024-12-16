using Microsoft.AspNetCore.Http;
using OrderManager.Domain.Constants;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using System.Security.Claims;

namespace OrderManager.Domain.Queries
{
    public class OrderQueryBuilder : IQueryableBuilder<Order>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderQueryBuilder(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IQueryable<Order> Build(IQueryable<Order> baseQuery)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");
            if (user.IsInRole(Roles.Admin) || user.IsInRole(Roles.SuperAdmin))
            {
                return baseQuery; // Admin and SuperAdmin see all orders
            }
            if (user.IsInRole(Roles.Customer))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return baseQuery.Where(o => o.CustomerId == Guid.Parse(userId));
            }

            if (user.IsInRole(Roles.RestaurantStaff))
            {
                return baseQuery.Where(o =>
                    o.State == OrderState.Pending ||
                    o.State == OrderState.Preparing ||
                    o.State == OrderState.ReadyForDelivery);
            }

            if (user.IsInRole(Roles.DeliveryStaff))
            {
                return baseQuery.Where(o =>
                    o.State == OrderState.ReadyForDelivery ||
                    o.State == OrderState.OutForDelivery ||
                    o.State == OrderState.Delivered ||
                    o.State == OrderState.UnableToDeliver);
            }

            throw new InvalidOperationException("User role is not recognized.");
        }
    }
}

