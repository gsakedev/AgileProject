namespace OrderManager.Domain.Interfaces
{
    public interface IQueryFilterFactory<T>
    {
        IEnumerable<IQueryFilter<T>> CreateFilters(object parameters);

    }
}
