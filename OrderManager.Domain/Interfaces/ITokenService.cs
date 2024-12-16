namespace OrderManager.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, IEnumerable<string> roles);
    }
}
