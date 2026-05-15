namespace RestaurantOrders.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using MediatR;
using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator? mediator = null)
    : DbContext(options), IUnitOfWork
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

    /// <summary>
    /// Ponto único de persistência: atualiza timestamps, despacha domain events e salva.
    /// Chamado exclusivamente pelo <c>TransactionBehavior</c> ao final de cada command.
    /// </summary>
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

        // Coleta e limpa os events antes de publicar para evitar reentrada
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

    // IUnitOfWork — ponto de commit explícito chamado pelo TransactionBehavior
    public Task<int> CommitAsync(CancellationToken ct = default) => SaveChangesAsync(ct);
}
