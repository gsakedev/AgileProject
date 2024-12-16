using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManager.Application.Commands;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Services
{//TODO: Needs more implementation
    public interface IDeliveryAssignmentLogic
    {
        Task AssignDeliveryAsync(Guid orderId);
    }
    public class NearestDeliveryAssignmentLogic : IDeliveryAssignmentLogic
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<DeliveryStaff> _deliveryStaffRepository;
        private readonly IMediator _mediator;

        public NearestDeliveryAssignmentLogic(IRepository<Order> orderRepository, IRepository<DeliveryStaff> deliveryStaffRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _deliveryStaffRepository = deliveryStaffRepository;
            _mediator = mediator;
        }

        public async Task AssignDeliveryAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, CancellationToken.None);
            if (order == null || order.State != OrderState.ReadyForDelivery)
            {
                throw new InvalidOperationException("Order is not ready for delivery.");
            }

            var availableStaff = await _deliveryStaffRepository.Query()
                .Where(ds => ds.IsAvailable)
                .OrderBy(ds => ds.CurrentLocation)
                .FirstOrDefaultAsync();


            if (availableStaff == null)
            {
                throw new InvalidOperationException("No delivery staff available.");
            }
            //TODO: TO CHEK THE DISTANCE BETWEEN ORDER AND AVAILBALE DELIVERYUSTAFF

            await _mediator.Send(new ChangeDeliveryStaffStatusCommand { DeliveryStaffId = availableStaff.Id, IsAvailable = false, OrderId = order.Id});


            order.State = OrderState.OutForDelivery;

            await _deliveryStaffRepository.SaveChangesAsync(CancellationToken.None);
            await _orderRepository.SaveChangesAsync(CancellationToken.None);
        }
    }
}