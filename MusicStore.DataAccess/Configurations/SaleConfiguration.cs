using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;

namespace MusicStore.DataAccess.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        // Fluent API
        builder.Property(p => p.OperationNumber)
            .HasMaxLength(10)
            .IsUnicode(false);

        builder.Property(p => p.Total)
            .HasPrecision(11, 2);
    }
}