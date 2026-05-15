namespace RestaurantOrders.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Enums;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(o => o.Notes).HasMaxLength(500);
        builder.Property(o => o.PlacedAt);
        builder.Property(o => o.ConfirmedAt);
        builder.Property(o => o.ReadyAt);
        builder.Property(o => o.DeliveredAt);

        builder.OwnsOne(o => o.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("total_amount")
                 .HasPrecision(10, 2)
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("currency")
                 .HasMaxLength(3)
                 .HasDefaultValue("BRL")
                 .IsRequired();
        });

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.TableId);
        builder.HasIndex(o => new { o.RestaurantId, o.Status });
    }
}
