namespace OrderManager.Domain.Interfaces
{
    public interface IAdminService
    {
        Task<double> GetAverageDeliveryTimeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        Task<int> GetDeliveredOrdersCountByStaffAsync(Guid staffId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    }
}
