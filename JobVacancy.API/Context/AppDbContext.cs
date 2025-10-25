using JobVacancy.API.models.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options): IdentityDbContext<UserEntity, RoleEntity, string>(options)
{ 
    public new DbSet<UserEntity> Users { get; set; }
    public new DbSet<RoleEntity> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserEntity>().ToTable("app_users");
        modelBuilder.Entity<RoleEntity>().ToTable("app_roles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("app_user_claims");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("app_user_roles");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("app_user_logins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("app_role_claims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("app_user_tokens");
    }
}