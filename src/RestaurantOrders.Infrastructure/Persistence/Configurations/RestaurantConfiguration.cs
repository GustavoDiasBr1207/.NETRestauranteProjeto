namespace RestaurantOrders.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrders.Domain.Entities;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable("restaurants");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).HasMaxLength(200).IsRequired();
        builder.Property(r => r.Slug).HasMaxLength(100).IsRequired();
        builder.Property(r => r.LogoUrl).HasMaxLength(1000);
        builder.Property(r => r.IsActive).HasDefaultValue(true);

        builder.HasIndex(r => r.Slug).IsUnique();

        builder.HasMany(r => r.Tables)
            .WithOne(t => t.Restaurant)
            .HasForeignKey(t => t.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Categories)
            .WithOne(c => c.Restaurant)
            .HasForeignKey(c => c.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
