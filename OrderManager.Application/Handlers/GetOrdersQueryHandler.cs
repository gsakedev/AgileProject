using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManager.Application.Mappers;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Queries;

namespace OrderManager.Application.Handlers
{
    public class GetOrdersQuery : IRequest<PagedResult<OrderDTO>>
    {
        public OrderQueryParameters Parameters { get; set; }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PagedResult<OrderDTO>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PagedResult<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var filteredQuery = _orderRepository.Query();

            // Apply Filters
            filteredQuery = filteredQuery.ApplyFilters(o =>
                (!request.Parameters.CustomerId.HasValue || o.CustomerId == request.Parameters.CustomerId) &&
                (!request.Parameters.DeliveryOption.HasValue || o.DeliveryOption == request.Parameters.DeliveryOption) &&
                (!request.Parameters.Price.HasValue || o.OrderTotalPrice == request.Parameters.Price) &&
                (!request.Parameters.State.HasValue || o.State == request.Parameters.State) &&
                (!request.Parameters.IsCompleted.HasValue || o.IsCompleted == request.Parameters.IsCompleted));


            // Apply Sorting and Paging
            var sortedAndPagedQuery = filteredQuery

                .ApplySorting(request.Parameters.SortBy, request.Parameters.IsDescending)
                .ApplyPaging(request.Parameters.Page, request.Parameters.PageSize);

            // Return results with total count
            var totalItems = await filteredQuery.CountAsync(cancellationToken);
            var items = await sortedAndPagedQuery.ToListAsync(cancellationToken);
            var dtoItems = items.Select(OrderMapper.ToOrderDTO).ToList();

            return new PagedResult<OrderDTO>(dtoItems, totalItems, request.Parameters.Page, request.Parameters.PageSize);
        }
    }
}
