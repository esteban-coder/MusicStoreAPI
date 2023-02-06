using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MusicStore.Entities.Infos;

namespace MusicStore.DataAccess
{
    public class MusicStoreDbContext : IdentityDbContext<MusicStoreUserIdentity>
    {
        public MusicStoreDbContext(DbContextOptions<MusicStoreDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            modelBuilder.Entity<SaleInfo>()
                .HasNoKey() // Aqui le decimos a EF Core de que no es un tabla
                .Property(p => p.Total)
                .HasPrecision(11, 2);
        }
    }
}