using Microsoft.AspNetCore.Http;
using OrderManager.Domain.Constants;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Domain.Helpers
{
    public class RoleBasedMenuItemFilter : IQueryFilter<MenuItem>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleBasedMenuItemFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IQueryable<MenuItem> Apply(IQueryable<MenuItem> query)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            if (user.IsInRole(Roles.Admin) || user.IsInRole(Roles.RestaurantStaff) || user.IsInRole(Roles.SuperAdmin))
            {
                return query; 
            }

            if (user.IsInRole(Roles.Customer))
            {
                return query.Where(m => m.IsAvailable);
            }

            throw new InvalidOperationException("User role is not recognized.");
        }
    }
}
