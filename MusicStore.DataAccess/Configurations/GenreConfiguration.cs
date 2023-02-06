using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;

namespace MusicStore.DataAccess.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder
            .Property(p => p.Name)
            .HasMaxLength(150);

        builder.HasData(new List<Genre>
        {
            new Genre() { Id = 1, Name = "Rock" },
            new Genre() { Id = 2, Name = "Pop" },
            new Genre() { Id = 3, Name = "Jazz" },
            new Genre() { Id = 4, Name = "Metal" },
            new Genre() { Id = 5, Name = "Disco" },
            new Genre() { Id = 6, Name = "Blues" },
            new Genre() { Id = 7, Name = "Reggae" },
            new Genre() { Id = 8, Name = "Reggaeton" },
        });
    }
}