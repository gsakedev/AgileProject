using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;
using OrderManager.Persistence.Contexts;
using OrderManager.Persistence.Identity;
using OrderManager.Persistence.Repositories;
using OrderManager.Persistence.Services;
using OrderManager.Shared.Interfaces;

namespace OrderManager.Persistence.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with the DefaultConnection string
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRepository<MenuItem>, Repository<MenuItem>>();
            services.AddScoped<IDeliveryStaffRepository, DeliveryStaffRepository>();
            services.AddScoped<IAdvancedRepository<Order>, AdvancedRepository<Order>>();

            return services;
        }
    }
}
