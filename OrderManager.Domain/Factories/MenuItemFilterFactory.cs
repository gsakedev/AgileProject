using Microsoft.AspNetCore.Http;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Queries;

namespace OrderManager.Domain.Factories
{

    public class MenuItemFilterFactory : IQueryFilterFactory<MenuItem>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MenuItemFilterFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IEnumerable<IQueryFilter<MenuItem>> CreateFilters(object parameterss)
        {
            var parameters = parameterss as MenuItemQueryParameters;

            var filters = new List<IQueryFilter<MenuItem>>();
            filters.Add(new RoleBasedMenuItemFilter(_httpContextAccessor));
            filters.Add(new PropertyBasedMenuItemFilter(parameters.Name, parameters.MinPrice, parameters.MaxPrice, parameters.SearchTerm));

            return filters;
        }
    }
}
