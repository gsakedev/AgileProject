using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManager.Application.Handlers;
using OrderManager.Application.Helpers;
using OrderManager.Application.Services;
using OrderManager.Application.Validators;
using OrderManager.Domain;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Factories;
using OrderManager.Domain.Helpers;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.Queries;
using System.Text.Json;

namespace OrderManager.Application.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddScoped(services, configuration);
            AddValidators(services, configuration);
            AddRabbitMQ(services, configuration);
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PlaceOrderHandler).Assembly));
            services.AddHttpContextAccessor();


            services.Configure<JsonSerializerOptions>(options =>
            {
                options.Converters.Add(new DeliveryOptionJsonConverter());
            });

            services.Configure<JsonSerializerOptions>(options =>
            {
                options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            return services;
        }

        private static IServiceCollection AddScoped(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderFactory, OrderFactory>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IMenuItemFactory, MenuItemFactory>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IQueryableBuilder<Order>, OrderQueryBuilder>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
            services.AddScoped<IQueryFilterFactory<MenuItem>, MenuItemFilterFactory>();
            services.AddScoped<IQueryFilterFactory<DeliveryStaff>, DeliveryStaffFilterFactory>();
            services.AddSingleton<IRabbitMqInitializer, RabbitMqInitializer>();

            return services;
        }
        private static IServiceCollection AddValidators(this IServiceCollection services, IConfiguration configuration)

        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<CreateMenuItemDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<PlaceOrderBusinessValidator>();
            services.AddValidatorsFromAssemblyContaining<PlaceOrderDTOValidator>();
            services.AddFluentValidationAutoValidation();
            return services;
        }
        private static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {

            var rabbitMqHost = configuration["RabbitMQ:HostName"] ?? "rabbitmq";
            var rabbitMqPort = int.Parse(configuration["RabbitMQ:Port"] ?? "5672");
            var rabbitMqUser = configuration["RabbitMQ:UserName"] ?? "guest";
            var rabbitMqPass = configuration["RabbitMQ:Password"] ?? "guest";
            services.AddSingleton<IMessagePublisher>(sp =>
                new RabbitMqPublisher(rabbitMqHost, rabbitMqPort, rabbitMqUser, rabbitMqPass));
            return services;

        }
    }
}
