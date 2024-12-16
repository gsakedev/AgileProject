using System.Linq.Expressions;

namespace OrderManager.Domain.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, Expression<Func<T, bool>>? filter = null)
        {
            return filter == null ? query : query.Where(filter);
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string sortBy, bool isDescending)
        {
            if (string.IsNullOrEmpty(sortBy)) return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, sortBy);

            var keySelector = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            return isDescending
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            return query.Skip(skip).Take(pageSize);
        }
    }
}
