using OrderManager.Domain.Specifications;
using System.Linq;
using System.Linq.Expressions;

namespace OrderManager.Domain.Interfaces
{
    public interface IAdvancedRepository<T> : IRepository<T> where T : class
    {
        IQueryable<TResult> QueryWithJoin<TEntity, TKey, TResult>(
            Expression<Func<T, bool>> where,
            Expression<Func<T, TKey>> outerKeySelector,
            Expression<Func<TEntity, TKey>> innerKeySelector,
            Expression<Func<T, TEntity, TResult>> resultSelector
        ) where TEntity : class;

        IQueryable<T> ApplySpecification(ISpecification<T> spec);
        public async Task<TResult> AggregateAsync<TResult>(
    IQueryable<TResult> query,
    Func<IQueryable<TResult>, Task<TResult>> aggregation)
        {
            return await aggregation(query);
        }
        IQueryable<T> ApplyPaging(IQueryable<T> query, int page, int pageSize);
        IQueryable<T> ApplySorting<TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector, bool descending);
    }
}
