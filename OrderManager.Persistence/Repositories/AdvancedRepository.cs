using Microsoft.EntityFrameworkCore;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Specifications;
using OrderManager.Persistence.Contexts;
using System.Linq.Expressions;

namespace OrderManager.Persistence.Repositories
{
    public class AdvancedRepository<T> : Repository<T>, IAdvancedRepository<T> where T : class
    {
        public AdvancedRepository(ApplicationDbContext dbContext) : base(dbContext) { }



        public IQueryable<TResult> QueryWithJoin<TEntity, TKey, TResult>(Expression<Func<T, bool>> where,
            Expression<Func<T, TKey>> outerKeySelector, 
            Expression<Func<TEntity, TKey>> innerKeySelector, 
            Expression<Func<T, TEntity, TResult>> resultSelector) where TEntity : class
        {


            return _dbContext.Set<T>()
                .Where(where)
                .Join(_dbContext.Set<TEntity>(), outerKeySelector, innerKeySelector, resultSelector);
        }
        public IQueryable<T> ApplyPaging(IQueryable<T> query, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            return query.Skip(skip).Take(pageSize);
        }

        public IQueryable<T> ApplySorting<TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector, bool descending)
        {
            return descending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
        }
        public IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            // Apply filters
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            // Apply includes
            foreach (var include in spec.Includes)
                query = query.Include(include);

            // Apply ordering
            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            // Apply paging
            if (spec.IsPagingEnabled)
                query = query.Skip(spec.Skip!.Value).Take(spec.Take!.Value);

            return query;
        }
    }
}
