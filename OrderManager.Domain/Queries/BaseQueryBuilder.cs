using OrderManager.Domain.Interfaces;
using System.Linq.Expressions;

namespace OrderManager.Domain.Queries
{
    public abstract class BaseQueryBuilder<T> : IQueryableBuilder<T>
    {
        private readonly List<Expression<Func<T, bool>>> _filters = new();

        public void AddFilter(Expression<Func<T, bool>> filter)
        {
            _filters.Add(filter);
        }

        public IQueryable<T> Build(IQueryable<T> query)
        {
            foreach (var filter in _filters)
            {
                query = query.Where(filter);
            }
            return query;
        }
    }
}
