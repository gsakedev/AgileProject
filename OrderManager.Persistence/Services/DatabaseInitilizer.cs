using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManager.Domain.Constants;
using OrderManager.Domain.Entities;
using OrderManager.Persistence.Contexts;
using OrderManager.Persistence.Identity;
using OrderManager.Shared.Interfaces;

namespace OrderManager.Persistence.Services
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DatabaseInitializer> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DatabaseInitializer(ApplicationDbContext dbContext, ILogger<DatabaseInitializer> logger,
            RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task InitializeAsync()
        {
            const int maxRetries = 5; 
            const int delayInSeconds = 5; 
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _logger.LogInformation("Attempting to apply database migrations (Attempt {Attempt}/{MaxRetries})...", attempt, maxRetries);
                    await _dbContext.Database.MigrateAsync(); // Apply pending migrations
                    _logger.LogInformation("Database migrations applied successfully.");

                    // Seed data after migrations
                    await SeedDataAsync();
                    break; 
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Database migration failed on attempt {Attempt}/{MaxRetries}.", attempt, maxRetries);

                    if (attempt == maxRetries)
                    {
                        _logger.LogCritical("Max retry attempts reached. Unable to initialize the database.");
                        throw; 
                    }

                    await Task.Delay(TimeSpan.FromSeconds(delayInSeconds)); 
                }
            }
        }

        private async Task SeedDataAsync()
        {
            var roles = Roles.GetAllRoles();

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            var superAdminEmail = "superadmin@example.com";
            const string adminPassword = "Admin@123";

            if (await _userManager.FindByEmailAsync(superAdminEmail) == null)
            {
                var superAdmin = new ApplicationUser
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    
                    Path = "superadmin"
                };

                await _userManager.CreateAsync(superAdmin, adminPassword);
                await _userManager.AddToRoleAsync(superAdmin, Roles.SuperAdmin);
            }
            // Seed Admin User
            const string adminEmail = "admin@ordermanager.com";

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Path = "superadmin.region1"
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
                }
                else
                {
                    _logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            var storeManagerEmail = "storemanager@example.com";
            if (await _userManager.FindByEmailAsync(storeManagerEmail) == null)
            {
                var storeManager = new ApplicationUser
                {
                    UserName = storeManagerEmail,
                    Email = storeManagerEmail,
                    Path = "superadmin.region1.store1",
                };

                await _userManager.CreateAsync(storeManager, adminPassword);
                await _userManager.AddToRoleAsync(storeManager, Roles.RestaurantStaff);
            }

            var restaurantStaff = "restaurantstaff@example.com";
            if (await _userManager.FindByEmailAsync(restaurantStaff) == null)
            {
                var storeManager = new ApplicationUser
                {
                    UserName = restaurantStaff,
                    Email = restaurantStaff,
                    Path = "superadmin.region1.store1",
                };

                await _userManager.CreateAsync(storeManager, adminPassword);
                await _userManager.AddToRoleAsync(storeManager, Roles.RestaurantStaff);
            }
            
            var deliveryStaffEmail = "deliverystaff1@example.com";
            if (await _userManager.FindByEmailAsync(deliveryStaffEmail) == null)
            {
                var deliveryStaff = new ApplicationUser
                {
                    UserName = deliveryStaffEmail,
                    Email = deliveryStaffEmail,
                    Path = "superadmin.region1.store1.delivery",
                };

                await _userManager.CreateAsync(deliveryStaff, adminPassword);
                await _userManager.AddToRoleAsync(deliveryStaff, Roles.DeliveryStaff);

                await _dbContext.DeliveryStaffs.AddAsync(new DeliveryStaff() 
                {
                     Id = deliveryStaff.Id,
                     User = deliveryStaff
                });

            }

            var deliveryStaff2Email = "deliverystaff2@example.com";
            if (await _userManager.FindByEmailAsync(deliveryStaff2Email) == null)
            {
                var deliveryStaff = new ApplicationUser
                {
                    UserName = deliveryStaff2Email,
                    Email = deliveryStaff2Email,
                    Path = "superadmin.region1.store1.delivery",
                };

                await _userManager.CreateAsync(deliveryStaff, adminPassword);
                await _userManager.AddToRoleAsync(deliveryStaff, Roles.DeliveryStaff);

                await _dbContext.DeliveryStaffs.AddAsync(new DeliveryStaff()
                {
                    Id = deliveryStaff.Id,
                    User = deliveryStaff
                });

            }
            var deliveryStaff3Email = "deliverystaff3@example.com";
            if (await _userManager.FindByEmailAsync(deliveryStaff3Email) == null)
            {
                var deliveryStaff = new ApplicationUser
                {
                    UserName = deliveryStaff3Email,
                    Email = deliveryStaff3Email,
                    Path = "superadmin.region1.store1.delivery",
                };

                await _userManager.CreateAsync(deliveryStaff, adminPassword);
                await _userManager.AddToRoleAsync(deliveryStaff, Roles.DeliveryStaff);

                await _dbContext.DeliveryStaffs.AddAsync(new DeliveryStaff()
                {
                    Id = deliveryStaff.Id,
                    User = deliveryStaff
                });
                                await _dbContext.SaveChangesAsync();

            }

            var customerEmail = "customer1@example.com";
            if (await _userManager.FindByEmailAsync(customerEmail) == null)
            {
                var customer = new ApplicationUser
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    Path = "customer",
                };

                await _userManager.CreateAsync(customer, adminPassword);
                await _userManager.AddToRoleAsync(customer, Roles.Customer);
            }

            var customer2Email = "customer2@example.com";
            if (await _userManager.FindByEmailAsync(customer2Email) == null)
            {
                var customer = new ApplicationUser
                {
                    UserName = customer2Email,
                    Email = customer2Email,
                    Path = "customer",
                };

                await _userManager.CreateAsync(customer, adminPassword);
                await _userManager.AddToRoleAsync(customer, Roles.Customer);
            }
            var customer3Email = "customer3@example.com";
            if (await _userManager.FindByEmailAsync(customer3Email) == null)
            {
                var customer = new ApplicationUser
                {
                    UserName = customer3Email,
                    Email = customer3Email,
                    Path = "customer",
                };

                await _userManager.CreateAsync(customer, adminPassword);
                await _userManager.AddToRoleAsync(customer, Roles.Customer);
            }

            if (!_dbContext.MenuItems.Any())
            {
                _logger.LogInformation("Seeding initial menu items...");

                _dbContext.MenuItems.AddRange(
                    new OrderManager.Domain.Entities.MenuItem { Name = "Pizza", Description = "Delicious cheese pizza", Price = 12.50m, IsAvailable = true },
                    new OrderManager.Domain.Entities.MenuItem { Name = "Burger", Description = "Juicy beef burger", Price = 8.99m, IsAvailable = true },
                    new OrderManager.Domain.Entities.MenuItem { Name = "Salad", Description = "Fresh garden salad", Price = 6.99m, IsAvailable = true },
                    new OrderManager.Domain.Entities.MenuItem { Name = "Pasta", Description = "Creamy Alfredo pasta", Price = 10.50m, IsAvailable = true },
                    new OrderManager.Domain.Entities.MenuItem { Name = "Soda", Description = "Refreshing cola drink", Price = 2.50m, IsAvailable = true }
                );

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Menu items seeded successfully.");
            }
            else
            {
                _logger.LogInformation("Menu items already exist. No seeding required.");

            }

        }
    }
}