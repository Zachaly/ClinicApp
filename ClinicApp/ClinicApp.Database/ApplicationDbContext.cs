using ClinicApp.Database.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Domain.Entity;

namespace ClinicApp.Database;

public class ApplicationDbContext : IdentityDbContext<
    DatabaseUser,
    IdentityRole<Guid>,
    Guid,
    IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<UserClaim> ApplicationUserClaims { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Drug> Drugs { get; set; }
    public DbSet<DrugClass> DrugClasses { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToView("vw_ApplicationUsers");
            entity.HasKey(u => u.Id);
        });

        builder.Entity<UserClaim>(entity =>
        {
            entity.ToView("vw_UserClaims");
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

        builder.Entity<Patient>(e =>
        {
            e.Property(e => e.LastName).HasMaxLength(50);
            e.Property(e => e.FirstName).HasMaxLength(50);
            e.Property(e => e.PostalCode).HasMaxLength(6);
            e.Property(e => e.Address).HasMaxLength(75);
            e.Property(e => e.City).HasMaxLength(50);
            e.Property(e => e.PeselNumber).HasMaxLength(11);
            e.HasQueryFilter(e => e.DeletedOn == null);
        });

        builder.Entity<DrugClass>(e =>
        {
            e.HasMany(c => c.Drugs)
                .WithOne(d => d.Class)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            e.Property(c => c.Name)
                .HasMaxLength(50);
        });

        builder.Entity<Drug>(e =>
        {
            e.Property(d => d.BrandName).HasMaxLength(50);
            e.Property(d => d.GenericName).HasMaxLength(50);
        });
    }
}
