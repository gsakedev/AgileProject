namespace OrderManager.Domain.Interfaces
{
    public interface IQueryFilter<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
