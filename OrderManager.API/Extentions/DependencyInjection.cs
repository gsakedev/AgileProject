using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderManager.API;
using OrderManager.Domain.Constants;
using OrderManager.Persistence.Contexts;
using OrderManager.Shared.Helpers;
using System.Text;

namespace OrderManager.Application.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthenticationExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
                        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                        var isBlacklisted = await dbContext.RevokedTokens.AnyAsync(t => t.Token == token);

                        if (isBlacklisted)
                        {
                            context.Fail("This token has been blacklisted.");
                        }
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.DeliveryStaffPolicy, policy =>
                    policy.RequireRole(Roles.DeliveryStaff, Roles.SuperAdmin));

                options.AddPolicy(Policies.CustomerPolicy, policy =>
                    policy.RequireRole(Roles.Customer, Roles.SuperAdmin));

                options.AddPolicy(Policies.RestaurantStaffPolicy, policy =>
    policy.RequireRole(Roles.RestaurantStaff, Roles.SuperAdmin));

                options.AddPolicy(Policies.AdminPolicy, policy =>
    policy.RequireRole(Roles.Admin, Roles.SuperAdmin));

                options.InvokeHandlersAfterFailure = true;
            });
            return services;
        }

        public static IServiceCollection AddSwaggerConf(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.SchemaFilter<EnumSchemaFilter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your valid token."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            return services;
        }
    }
}
