using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastracture;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastracture.Mail;
using Ordering.Infrastracture.Persistence;
using Ordering.Infrastracture.Repositories;
using SendGrid.Extensions.DependencyInjection;

namespace Ordering.Infrastracture;

public static class InfrastractureServiceRegistration
{
    public static IServiceCollection AddInfrastractureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderConnectionString")));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"))
            .AddSendGrid(options => options.ApiKey = configuration["EmailSettings:ApiKey"]);
        services.AddTransient<IEmailService, EmailService>();


        return services;
    }
}
