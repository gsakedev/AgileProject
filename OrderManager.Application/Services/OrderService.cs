using MediatR;
using Microsoft.AspNetCore.Http;
using OrderManager.Application.Commands;
using OrderManager.Application.Handlers;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Queries;
using System.Security.Claims;

namespace OrderManager.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Order> _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IMediator mediator, IRepository<Order> orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Guid> PlaceOrderAsync(PlaceOrderDTO dto, CancellationToken cancellationToken)
        {
            var command = new PlaceOrderCommand
            {
                DeliveryOption = Enum.Parse<Domain.Enums.DeliveryOption>(dto.DeliveryOption, true),
                SpecialInstructions = dto.SpecialInstructions,
                Items = dto.Items.Select(item => new OrderItemCommand
                {
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity
                }).ToList()
            };

            return await _mediator.Send(command, cancellationToken);
        }


        public async Task MoverOrderToNext(Guid orderId,  CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order == null || order.State.Equals(OrderState.ReadyForDelivery))
            {
                throw new InvalidOperationException($"This order Can't be moved");
            }
             Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,out Guid u);

            await _mediator.Send(new MoveOrderToNextPhaseCommand
            {
                OrderId = orderId,
                StaffId = u,
            });
        }

        public async Task CancelOred(Guid orderId, CancellationToken cancellationToken)
        {
            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid u);

            await _mediator.Send(new CancelOrderCommand
            {
                OrderId = orderId,
                StaffId = u
            }, cancellationToken);
        }

        public async Task<PagedResult<OrderDTO>> GetOrders(OrderQueryParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOrdersQuery { Parameters = parameters }, cancellationToken);
            return result;
        }

        public async Task UpdateDeliveryStatus(Guid orderId, bool delivered, CancellationToken cancellationToken)
        {
            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid u);
            await _mediator.Send(new UpdateDeliveryStatusCommand
            {
                OrderId = orderId,
                StaffId = u,
                Delivered = delivered
            }, cancellationToken);
        }

        public async Task AssignDelivery(Guid orderId, Guid deliveryStaffId, CancellationToken cancellationToken)
        {
            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid u);

            await _mediator.Send(new AssignDeliveryCommand
            {
                OrderId = orderId,
                DeliveryStaffId = deliveryStaffId,
            }, cancellationToken);
        }

        public async Task<PagedResult<DeliveryStaffDTO>> GetAvailableDeliveryStaff(DeliveryStaffQueryParameters parameters, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            var result = await _mediator.Send(new GetAvailableDeliveryStaffQuery { Parameters = parameters }, cancellationToken);

            return result;
        }
    }
}
