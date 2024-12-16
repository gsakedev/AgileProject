using Microsoft.EntityFrameworkCore;
using OrderManager.Domain.Interfaces;
using OrderManager.Persistence.Contexts;
using System.Linq.Expressions;

namespace OrderManager.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>() ?? throw new InvalidOperationException($"DbSet for entity {typeof(T).Name} is not configured.");

        }
        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public virtual IQueryable<T> Query()
        {
            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                return _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false).AsNoTracking();
            }
            return _dbSet.AsNoTracking();
        }
        public virtual async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);
        }
        public async Task SoftDelete(T entity)
        {
            if (entity.GetType().GetProperty("IsDeleted") == null)
            {
                throw new InvalidOperationException($"Entity {typeof(T).Name} does not have an 'IsDeleted' property.");
            }

            var isDeletedProperty = entity.GetType().GetProperty("IsDeleted");
            isDeletedProperty.SetValue(entity, true);
        }

        public async Task<T> GetByIdStringAsync(string id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
        }
    }
}
