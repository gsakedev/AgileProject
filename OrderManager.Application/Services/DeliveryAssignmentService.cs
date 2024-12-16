using Microsoft.Extensions.Hosting;
using OrderManager.Domain.Events;

namespace OrderManager.Application.Services
{
    public class DeliveryAssignmentService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
