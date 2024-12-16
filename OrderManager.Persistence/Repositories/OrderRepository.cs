using Microsoft.EntityFrameworkCore;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;
using OrderManager.Persistence.Contexts;

namespace OrderManager.Persistence.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly IQueryableBuilder<Order> _queryBuilder;
        public OrderRepository(ApplicationDbContext dbContext, IQueryableBuilder<Order> queryBuilder)
            : base(dbContext)
        {
            _queryBuilder = queryBuilder;
        }
        public override IQueryable<Order> Query()
        {
            var baseQuery = _dbContext.Set<Order>()
                .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem);

            return _queryBuilder.Build(baseQuery); 
        }
        public override async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = Query(); 
            return await query.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }
    }
}
