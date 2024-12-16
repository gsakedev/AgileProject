using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Specifications;

namespace OrderManager.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdvancedRepository<Order> _orderRepository;
        public AdminService(IAdvancedRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<double> GetAverageDeliveryTimeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {

            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
            var spec = new DeliveredOrdersSpecification(startDate, endDate);
            var orders = await _orderRepository.ApplySpecification(spec).ToListAsync(cancellationToken);

            if (!orders.Any())
                return 0;

            return orders.Average(o => (o.CompletedDate.Value - o.OrderDate).TotalMinutes);
        }

        public async Task<int> GetDeliveredOrdersCountByStaffAsync(Guid staffId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            throw   new NotImplementedException();
            //var spec = new DeliveryStaffOrdersSpecification(startDate, endDate, staffId);
            //return await _orderRepository.ApplySpecification(spec).CountAsync(cancellationToken);
        }
    }
}
