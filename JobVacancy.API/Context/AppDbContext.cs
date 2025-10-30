using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasOne(e => e.Enterprise)
                .WithOne(e => e.User);
        });
        
        modelBuilder.Entity<EnterpriseEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(250).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Description).IsRequired(false).HasColumnType("TEXT");
            entity.Property(e => e.WebSiteUrl).IsRequired(false).HasColumnType("TEXT");
            entity.Property(e => e.LogoUrl).IsRequired(false).HasColumnType("TEXT");

            var converter = new EnumToStringConverter<EnterpriseTypeEnum>();
            
            modelBuilder.Entity<EnterpriseEntity>()
                .Property(e => e.Type)
                .HasConversion(converter);
            
            modelBuilder.Entity<EnterpriseEntity>()
                .Property(e => e.Type)
                .HasMaxLength(60);

            entity.HasOne(e => e.User)          
                .WithOne(u => u.Enterprise)    
                .HasForeignKey<EnterpriseEntity>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EnterpriseIndustryEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.IsPrimary);
            entity.Property(e => e.IsPrimary).IsRequired();
            
            entity.HasOne(ei => ei.Enterprise)
                .WithMany(e => e.IndustryLinks) 
                .HasForeignKey(ei => ei.EnterpriseId);

            entity.HasOne(ei => ei.Industry)
                .WithMany(i => i.EnterpriseLinks) 
                .HasForeignKey(ei => ei.IndustryId);
        });
        
        modelBuilder.Entity<IndustryEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Description).HasColumnType("TEXT").IsRequired(false);
            entity.Property(e => e.IconUrl).HasColumnType("TEXT").IsRequired(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });
        
        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
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