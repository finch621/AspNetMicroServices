using Npgsql;

namespace Discount.API.Extensions;

public static class ServiceProviderExtensions
{
    public static void MigrateDatabase<TContext>(this IServiceProvider service, int? retry = 0)
    {
        var retryForAvailability = retry ?? 0;
        var configuration = service.GetRequiredService<IConfiguration>();
        var logger = service.GetRequiredService<ILogger<TContext>>();

        try
        {
            var connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            logger.LogInformation($"Migrating psql database with connection string:{connectionString}");
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand
            {
                Connection = connection
            };

            command.CommandText = "drop table if exists coupon";
            command.ExecuteNonQuery();

            command.CommandText = @"create table coupon(Id serial primary key,
                                                        ProductName varchar(24) not null,
                                                        Description text,
                                                        Amount int)";
            command.ExecuteNonQuery();

            command.CommandText = @"insert into coupon(productname, description, amount)
                                            values('Iphone X', 'Iphone discount', 150)";
            command.ExecuteNonQuery();

            command.CommandText = @"insert into coupon(productname, description, amount)
                                            values('Samsung 10', 'Samsung discount', 100)";
            command.ExecuteNonQuery();

            command.CommandText = @"insert into coupon(productname, description, amount)
                                            values('Realme 11 Pro', 'Realme discount', 200)";
            command.ExecuteNonQuery();

            logger.LogInformation("Done migrating psql database.");
        }
        catch (NpgsqlException e)
        {
            logger.LogError(e, "An error occured while migrating psql database!");
            if (retryForAvailability < 50)
            {
                retryForAvailability++;
                Thread.Sleep(2000);
                MigrateDatabase<TContext>(service, retryForAvailability);
            }
        }
    }
}
