namespace RestaurantOrders.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrders.Domain.Entities;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name).HasMaxLength(200).IsRequired();
        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.Notes).HasMaxLength(500);

        // Snapshot do preço unitário
        builder.OwnsOne(i => i.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("unit_price")
                 .HasPrecision(10, 2)
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("unit_price_currency")
                 .HasMaxLength(3)
                 .HasDefaultValue("BRL")
                 .IsRequired();
        });

        // Snapshot do subtotal
        builder.OwnsOne(i => i.Subtotal, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("subtotal")
                 .HasPrecision(10, 2)
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("subtotal_currency")
                 .HasMaxLength(3)
                 .HasDefaultValue("BRL")
                 .IsRequired();
        });

        builder.HasIndex(i => i.OrderId);
        builder.HasIndex(i => i.MenuItemId);
    }
}
