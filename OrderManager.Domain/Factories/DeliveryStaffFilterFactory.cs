using Microsoft.AspNetCore.Http;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Factories
{
    public class DeliveryStaffFilterFactory : IQueryFilterFactory<DeliveryStaff>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeliveryStaffFilterFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IEnumerable<IQueryFilter<DeliveryStaff>> CreateFilters(object parameterss)
        {
            var parameters = parameterss as DeliveryStaffQueryParameters;

            var filters = new List<IQueryFilter<DeliveryStaff>>();
            filters.Add(new PropertyBasedDeliveryStaffFilter(parameters.id, parameters.isAvailable, parameters.location));
            filters.Add(new RoleBasedDeliveryStaffFilter(_httpContextAccessor));

            return filters;
        }
    }
}
