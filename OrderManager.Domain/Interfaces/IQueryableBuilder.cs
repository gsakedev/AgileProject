namespace OrderManager.Domain.Interfaces
{
    public interface IQueryableBuilder<T>
    {    /// <summary>
         /// Applies query filters and transformations based on the context (e.g., roles, policies).
         /// </summary>
         /// <param name="query">The base query.</param>
         /// <returns>The modified query.</returns>
         /// 
        IQueryable<T> Build(IQueryable<T> query);
    }
}
