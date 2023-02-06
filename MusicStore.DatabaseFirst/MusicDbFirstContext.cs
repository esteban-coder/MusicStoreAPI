using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MusicStore.DatabaseFirst
{
    public partial class MusicDbFirstContext : DbContext
    {
        public MusicDbFirstContext()
        {
        }

        public MusicDbFirstContext(DbContextOptions<MusicDbFirstContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Sale> Sales { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.FullName).HasMaxLength(200);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasIndex(e => e.GenreId, "IX_Events_GenreId");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Place).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(11, 2)");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.GenreId);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sale");

                entity.HasIndex(e => e.ConcertId, "IX_Sale_ConcertId");

                entity.HasIndex(e => e.CustomerForeignKey, "IX_Sale_CustomerForeignKey");

                entity.Property(e => e.OperationNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("decimal(11, 2)");

                entity.HasOne(d => d.Concert)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.ConcertId);

                entity.HasOne(d => d.CustomerForeignKeyNavigation)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.CustomerForeignKey);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
