using Microsoft.AspNetCore.Http;
using OrderManager.Domain.Constants;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;
using System.Runtime.ConstrainedExecution;

namespace OrderManager.Domain.Helpers
{
    public class RoleBasedDeliveryStaffFilter : IQueryFilter<DeliveryStaff>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleBasedDeliveryStaffFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IQueryable<DeliveryStaff> Apply(IQueryable<DeliveryStaff> query)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            if (user.IsInRole(Roles.Admin) || user.IsInRole(Roles.RestaurantStaff) || user.IsInRole(Roles.SuperAdmin))
            {
                return query; 
            }
            throw new InvalidOperationException("User role is not recognized.");
        }
    }
}
