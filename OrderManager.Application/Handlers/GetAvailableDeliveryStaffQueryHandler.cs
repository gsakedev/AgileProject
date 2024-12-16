using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManager.Application.Handlers;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Domain.Queries
{
    public class GetAvailableDeliveryStaffQuery : IRequest<PagedResult<DeliveryStaffDTO>>
    {
        public DeliveryStaffQueryParameters Parameters { get; set; }
    }
    public class GetAvailableDeliveryStaffQueryHandler : IRequestHandler<GetAvailableDeliveryStaffQuery, PagedResult<DeliveryStaffDTO>>
    {
        private readonly IRepository<DeliveryStaff> _deliveryStaffRepostiry;
        private readonly IQueryFilterFactory<DeliveryStaff> _filterFactory;

        public GetAvailableDeliveryStaffQueryHandler(IRepository<DeliveryStaff> deliveryStaffRepostiry, IQueryFilterFactory<DeliveryStaff> filterFactory)
        {
            _deliveryStaffRepostiry = deliveryStaffRepostiry;
            _filterFactory = filterFactory;
        }
        public async Task<PagedResult<DeliveryStaffDTO>> Handle(GetAvailableDeliveryStaffQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _deliveryStaffRepostiry.Query();
            var filters = _filterFactory.CreateFilters(request.Parameters);
            foreach (var filter in filters)
            {
                baseQuery = filter.Apply(baseQuery);
            }

            baseQuery = baseQuery
    .ApplySorting(request.Parameters.SortBy, request.Parameters.IsDescending)
    .ApplyPaging(request.Parameters.Page, request.Parameters.PageSize);
            var totalCount = await baseQuery.CountAsync(cancellationToken);
            var deliveryStaff = await baseQuery.ToListAsync(cancellationToken);

            var dto = deliveryStaff.Select(d => new DeliveryStaffDTO
            { CurrentLocation = d.CurrentLocation,
              CurrentOrderId = d.CurrentOrderId,
               Id = d.Id,
                IsAvailable = d.IsAvailable
            }).ToList();
            return new PagedResult<DeliveryStaffDTO>(dto, totalCount, request.Parameters.Page, request.Parameters.PageSize);

        }
    }
}
