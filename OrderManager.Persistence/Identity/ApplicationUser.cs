using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Persistence.Identity
{
    public class ApplicationUser : IdentityUser, IUser
    {
        public LTree Path { get; set; } 
    }
}
