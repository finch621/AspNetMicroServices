using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider MigrateDabase<TContext>(this IServiceProvider services,
            Action<TContext, IServiceProvider> seeder,
            int? retry = 0) where TContext : DbContext
    {
        var retryForAvailability = retry ?? 0;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetService<TContext>();


        try
        {
            if (context == null)
                throw new NullReferenceException($"{typeof(TContext).Name} is null.");

            logger.LogInformation($"Migrating database associated with {typeof(TContext).Name}.");

            InvokeSeeder<TContext>(seeder, context, services);

            logger.LogInformation($"Migrated database associated with {typeof(TContext).Name}.");
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, $"Migrating database associated with {typeof(TContext).Name} failed.");
            if (retryForAvailability < 50)
            {
                retryForAvailability++;
                logger.LogInformation($"Retrying for {retryForAvailability} times..");
                Thread.Sleep(2000);
                MigrateDabase<TContext>(services, seeder, retryForAvailability);
            }
        }

        return services;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
            TContext context,
            IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}
