using Microsoft.EntityFrameworkCore;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;
using OrderManager.Persistence.Contexts;

namespace OrderManager.Persistence.Repositories
{
    public class DeliveryStaffRepository : Repository<DeliveryStaff>, IDeliveryStaffRepository
    {
        public DeliveryStaffRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        public async Task<IEnumerable<DeliveryStaff>> GetAvailableDeliveryStaffAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(ds => ds.User)
                .Where(ds => ds.IsAvailable)
                .ToListAsync(cancellationToken);
        }
        public async Task<DeliveryStaff?> GetDeliveryStaffByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(ds => ds.CurrentOrder)
                .FirstOrDefaultAsync(ds => ds.Id == id, cancellationToken);
        }
    }
}
