namespace OrderManager.Domain.Entities
{
    public class RevokedToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } 
        public DateTime RevokedAt { get; set; } = DateTime.UtcNow; 
        public DateTime Expiration { get; set; } 

    }
}
