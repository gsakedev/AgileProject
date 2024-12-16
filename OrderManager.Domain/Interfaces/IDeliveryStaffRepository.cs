using OrderManager.Domain.Entities;

namespace OrderManager.Domain.Interfaces
{
    public interface IDeliveryStaffRepository : IRepository<DeliveryStaff>
    {
        Task<IEnumerable<DeliveryStaff>> GetAvailableDeliveryStaffAsync(CancellationToken cancellationToken);
        Task<DeliveryStaff?> GetDeliveryStaffByIdAsync(string id, CancellationToken cancellationToken);

    }
}
