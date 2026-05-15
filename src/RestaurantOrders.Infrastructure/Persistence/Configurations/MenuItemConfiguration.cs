namespace RestaurantOrders.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrders.Domain.Entities;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("menu_items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name).HasMaxLength(200).IsRequired();
        builder.Property(i => i.Description).HasMaxLength(1000);
        builder.Property(i => i.ImageUrl).HasMaxLength(1000);
        builder.Property(i => i.IsAvailable).HasDefaultValue(true);
        builder.Property(i => i.DisplayOrder).HasDefaultValue(0);

        builder.OwnsOne(i => i.Price, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("price")
                 .HasPrecision(10, 2)
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("price_currency")
                 .HasMaxLength(3)
                 .HasDefaultValue("BRL")
                 .IsRequired();
        });

        builder.HasIndex(i => new { i.RestaurantId, i.IsAvailable });
        builder.HasIndex(i => i.CategoryId);
    }
}
