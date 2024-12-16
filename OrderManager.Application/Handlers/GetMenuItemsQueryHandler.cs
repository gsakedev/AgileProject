using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManager.Application.Mappers;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Queries;

namespace OrderManager.Application.Handlers
{
    public class GetMenuItemsQuery : IRequest<PagedResult<MenuItemDTO>>
    {
        public MenuItemQueryParameters Parameters { get; set; }
    }
    public class GetMenuItemsQueryHandler : IRequestHandler<GetMenuItemsQuery, PagedResult<MenuItemDTO>>
    {
        private readonly IRepository<MenuItem> _menuItemRepository;
        private readonly IQueryFilterFactory<MenuItem> _filterFactory;


        public GetMenuItemsQueryHandler(IRepository<MenuItem> menuItemRepository, IQueryFilterFactory<MenuItem> filterFactory)
        {
            _menuItemRepository = menuItemRepository;
            _filterFactory = filterFactory;
        }
        public async Task<PagedResult<MenuItemDTO>> Handle(GetMenuItemsQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _menuItemRepository.Query();

            var filters = _filterFactory.CreateFilters(request.Parameters);

            // Apply Filtering
            foreach (var filter in filters)
            {
                baseQuery = filter.Apply(baseQuery);
            }

            baseQuery = baseQuery
                .ApplySorting(request.Parameters.SortBy, request.Parameters.IsDescending)
                .ApplyPaging(request.Parameters.Page, request.Parameters.PageSize);

            var totalCount = await baseQuery.CountAsync(cancellationToken);
            var menuItems = await baseQuery.ToListAsync(cancellationToken);

            var dtoItems = menuItems.Select(MenuItemMapper.ToDTO).ToList();

            return new PagedResult<MenuItemDTO>(dtoItems, totalCount, request.Parameters.Page, request.Parameters.PageSize);
        }
    }
}
