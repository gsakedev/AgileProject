using Microsoft.AspNetCore.Mvc;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Queries;

namespace OrderManager.Domain.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> PlaceOrderAsync(PlaceOrderDTO dto, CancellationToken cancellationToken);
        Task MoverOrderToNext(Guid orderId, CancellationToken cancellationToken);
        Task CancelOred(Guid orderId, CancellationToken cancellationToken);
        Task<PagedResult<OrderDTO>> GetOrders(OrderQueryParameters parameters, CancellationToken cancellationToken);
        Task UpdateDeliveryStatus(Guid orderId, bool delivered, CancellationToken cancellationToken);
        Task AssignDelivery(Guid orderId, Guid deliveryStaffId, CancellationToken cancellationToken);
        Task<PagedResult<DeliveryStaffDTO>> GetAvailableDeliveryStaff(DeliveryStaffQueryParameters parameters, CancellationToken cancellationToken);
    }
}