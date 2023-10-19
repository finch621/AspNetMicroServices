using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastracture.Persistence;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Order>().Property(p => p.TotalPrice).HasPrecision(18, 2);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            entry.Entity.LastModifiedBy = "doffi";
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.LastModifiedStatus = LastModifiedStatusEnum.Created;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.Now;
                    entry.Entity.LastModifiedStatus = LastModifiedStatusEnum.Updated;
                    break;
                case EntityState.Deleted:
                    entry.Entity.DeletedAt = DateTime.Now;
                    entry.Entity.LastModifiedStatus = LastModifiedStatusEnum.Deleted;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
