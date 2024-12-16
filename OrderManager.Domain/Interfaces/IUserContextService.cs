namespace OrderManager.Domain.Interfaces
{
    public interface IUserContextService
    {
        Guid GetCurrentUserId();
    }
}
