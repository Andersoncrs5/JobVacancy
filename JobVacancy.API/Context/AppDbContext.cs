using JobVacancy.API.models.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobVacancy.API.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options): IdentityDbContext<UserEntity, RoleEntity, string>(options)
{ 
    public new DbSet<UserEntity> Users { get; set; }
    public new DbSet<RoleEntity> Roles { get; set; }
    public new DbSet<IndustryEntity> Industries { get; set; }
    public new DbSet<EnterpriseIndustryEntity> EnterpriseIndustries { get; set; }
    public new DbSet<EnterpriseEntity> Enterprises { get; set; }
    public new DbSet<CategoryEntity> Categories { get; set; }

    public override int SaveChanges()
    {
        SetAuditDates();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditDates();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditDates()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.Id = entry.Entity.Id ?? Guid.NewGuid().ToString();
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IndustryEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Description).HasColumnType("TEXT").IsRequired(false);
            entity.Property(e => e.IconUrl).HasColumnType("TEXT").IsRequired(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });
        
        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Description).HasColumnType("TEXT");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt);
            entity.Property(e => e.UpdatedAt);
        });
        
        modelBuilder.Entity<UserEntity>().ToTable("app_users");
        modelBuilder.Entity<RoleEntity>().ToTable("app_roles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("app_user_claims");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("app_user_roles");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("app_user_logins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("app_role_claims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("app_user_tokens");
    }
}