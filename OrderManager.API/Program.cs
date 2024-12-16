using MediatR;
using OrderManager.API.Middlewares;
using OrderManager.Application.Extentions;
using OrderManager.Domain;
using OrderManager.Persistence.Extentions;
using OrderManager.Shared.Interfaces;

namespace OrderManager.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Application is starting...");

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddAuthenticationExt(builder.Configuration);
            builder.Services.AddSwaggerConf(builder.Configuration);

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables(); 

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
                await databaseInitializer.InitializeAsync();
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Manager API v1");

                    c.OAuthClientId("client");
                    c.OAuthClientSecret("secret");
                });
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IRabbitMqInitializer>();
                await initializer.InitializeAsync(builder.Configuration);
            }
            app.Run();
        }
    }
}
