using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Domain.Helpers
{
    public class PropertyBasedMenuItemFilter : IQueryFilter<MenuItem>
    {
        private readonly string? _nameFilter;
        private readonly decimal? _minPrice;
        private readonly decimal? _maxPrice;
        private readonly string? _searchTerm;
        public PropertyBasedMenuItemFilter(string? nameFilter, decimal? minPrice, decimal? maxPrice, string? searchTerm)
        {
            _nameFilter = nameFilter;
            _minPrice = minPrice;
            _maxPrice = maxPrice;
            _searchTerm = searchTerm;   
        }
        public IQueryable<MenuItem> Apply(IQueryable<MenuItem> query)
        {
            if (!string.IsNullOrEmpty(_nameFilter))
            {
                query = query.Where(m => m.Name.Contains(_nameFilter));
            }

            if (_minPrice.HasValue)
            {
                query = query.Where(m => m.Price >= _minPrice.Value);
            }

            if (_maxPrice.HasValue)
            {
                query = query.Where(m => m.Price <= _maxPrice.Value);
            }
            if (_searchTerm is not null)
            {
                query = query.Where(m => m.Name.Contains(_searchTerm));
            }
            return query;
        }
    }
}
