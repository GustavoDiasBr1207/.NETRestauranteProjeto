namespace RestaurantOrders.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrders.Domain.Entities;

public class TableConfiguration : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.ToTable("tables");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Number).IsRequired();
        builder.Property(t => t.QrCodeUrl).HasMaxLength(1000);
        builder.Property(t => t.IsActive).HasDefaultValue(true);

        builder.OwnsOne(t => t.QrCode, qr =>
        {
            qr.Property(q => q.Token)
              .HasColumnName("qr_code_token")
              .HasMaxLength(100)
              .IsRequired();

            qr.HasIndex(q => q.Token).IsUnique();
        });

        builder.HasIndex(t => new { t.RestaurantId, t.Number }).IsUnique();
    }
}
