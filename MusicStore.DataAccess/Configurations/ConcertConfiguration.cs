using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;

namespace MusicStore.DataAccess.Configurations;

public class ConcertConfiguration : IEntityTypeConfiguration<Concert>
{
    public void Configure(EntityTypeBuilder<Concert> builder)
    {
        builder.Property(p => p.UnitPrice)
            .HasPrecision(11, 2);
    }
}