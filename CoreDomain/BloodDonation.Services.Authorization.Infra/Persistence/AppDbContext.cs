using BloodDonation.Services.Authorization.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloodDonation.Services.Authorization.Infra.Persistence
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new DbInitializer(modelBuilder).seed();

            base.OnModelCreating(modelBuilder);

            // Remover referências ao IdentityUser, se existirem
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName()!.ToLower());
            }
        }
    }
}
