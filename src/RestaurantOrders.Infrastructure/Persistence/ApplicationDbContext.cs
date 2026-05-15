namespace RestaurantOrders.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Common;
using MediatR;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator? mediator = null)
    : DbContext(options)
{
    public DbSet<Restaurant>   Restaurants    { get; set; } = null!;
    public DbSet<Table>        Tables         { get; set; } = null!;
    public DbSet<MenuCategory> MenuCategories { get; set; } = null!;
    public DbSet<MenuItem>     MenuItems      { get; set; } = null!;
    public DbSet<Customer>     Customers      { get; set; } = null!;
    public DbSet<Order>        Orders         { get; set; } = null!;
    public DbSet<OrderItem>    OrderItems     { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Atualiza campos de auditoria automaticamente
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;

            if (entry.State is EntityState.Added or EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        // Despacha domain events antes de persistir
        if (mediator is not null)
        {
            var entities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var events = entities.SelectMany(e => e.DomainEvents).ToList();
            entities.ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in events)
                await mediator.Publish(domainEvent, ct);
        }

        return await base.SaveChangesAsync(ct);
    }
}
