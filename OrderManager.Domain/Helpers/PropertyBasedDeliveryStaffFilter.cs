using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Domain.Helpers
{
    public class PropertyBasedDeliveryStaffFilter : IQueryFilter<DeliveryStaff>
    {
        private readonly string? _staffId;
        private readonly bool? _isAvailable;
        private readonly string? _location;
        public PropertyBasedDeliveryStaffFilter(string? staffId, bool? isAvalable, string? location)
        {
            _staffId = staffId;
            _isAvailable = isAvalable;
            _location = location;
        }
        public IQueryable<DeliveryStaff> Apply(IQueryable<DeliveryStaff> query)
        {
            if (!string.IsNullOrEmpty(_staffId))
            {
                query = query.Where(d => d.Id.Equals(_staffId));
            }

            if (_isAvailable.HasValue)
            {
                query = _isAvailable == true ? query.Where(d => d.IsAvailable) : query.Where(d => !d.IsAvailable);
            }
            return query;
        }
    }
}
