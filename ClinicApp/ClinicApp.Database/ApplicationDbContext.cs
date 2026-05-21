using ClinicApp.Database.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Domain.Entity;

namespace ClinicApp.Database;

public class ApplicationDbContext : IdentityDbContext<DatabaseUser, IdentityRole<Guid>, Guid>
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<UserClaim> ApplicationUserClaims { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
            entity.HasKey(u => u.Id);
        });

        builder.Entity<UserClaim>(entity =>
        {
            entity.ToTable("AspNetUserClaims", t => t.ExcludeFromMigrations());
            entity.HasKey(c => c.Id);

            entity.HasOne<ApplicationUser>()
                  .WithMany(u => u.Claims)
                  .HasForeignKey(c => c.UserId);
        });

        builder.Entity<DatabaseUser>()
            .Property(u => u.FirstName)
            .HasMaxLength(50);

        builder.Entity<DatabaseUser>()
            .Property(u => u.LastName)
            .HasMaxLength(50);

        builder.Entity<DatabaseUser>()
            .HasKey(u => u.Id);
    }
}
