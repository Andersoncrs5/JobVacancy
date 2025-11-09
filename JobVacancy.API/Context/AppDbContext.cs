using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Base;
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
    public new DbSet<PostUserEntity> PostUser { get; set;  }
    public new DbSet<PostEnterpriseEntity> PostEnterprise { get; set;  }
    public new DbSet<SkillEntity> Skill { get; set;  }
    public new DbSet<UserSkillEntity> UserSkill { get; set; }
    public new DbSet<FavoritePostUserEntity> FavoritePostUser { get; set; }
    public new DbSet<FavoritePostEnterpriseEntity> FavoritePostEnterprise { get; set; }
    public new DbSet<CommentPostUserEntity> CommentPostUser { get; set; }
    public new DbSet<CommentPostEnterpriseEntity> CommentPostEnterprise { get; set; }
    public new DbSet<FavoriteCommentEntity> FavoriteCommentEntities { get; set; }
    public new DbSet<EmployeeInvitationEntity> EmployeeInvitations { get; set; }
    public new DbSet<PositionEntity> Positions { get; set; }
    public new DbSet<EmployeeEnterpriseEntity> EmployeeEnterprises { get; set; }
    
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
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EmployeeEnterpriseEntity>(options =>
        {
            options.HasKey(x => x.Id);
            options.Property(x => x.ContractLink).HasMaxLength(1000).IsRequired(false);
            options.Property(x => x.SalaryRange).HasMaxLength(200).IsRequired();
            options.Property(x => x.TerminationReason).HasMaxLength(600).IsRequired(false);
            options.Property(x => x.Notes).HasMaxLength(600).IsRequired(false);
            options.Property(x => x.SalaryValue).IsRequired();
            
            options.Property(x => x.PaymentFrequency).IsRequired();
            options.Property(x => x.ContractLegalType).IsRequired(false);
            options.Property(x => x.EmploymentType).IsRequired();
            options.Property(x => x.EmploymentStatus).IsRequired();
            options.Property(x => x.Currency).IsRequired();
            
            options.Property(x => x.EndDate).IsRequired(false);
            
            options.HasOne(x => x.Position)
                .WithMany(x => x.EmployeeEnterprise)
                .HasForeignKey(x => x.PositionId)
                .IsRequired();
            
            options.HasOne(x => x.Enterprise)
                .WithMany(x => x.EmployeeEnterprise)
                .HasForeignKey(x => x.EnterpriseId)
                .IsRequired();
            
            options.HasOne(x => x.User)
                .WithMany(e => e.Employee)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
            
            options.HasOne(x => x.InviteSender)
                .WithMany(e => e.InvitationsEnterprise)
                .HasForeignKey(x => x.InviteSenderId)
                .IsRequired();
        });
        
        modelBuilder.Entity<EmployeeInvitationEntity>(options =>
        {
            options.ToTable("EmployeeInvitations");
            
            options.HasKey(ev => ev.Id);
            
            options.HasIndex(ev => ev.UserId);
            options.HasIndex(ev => ev.EnterpriseId);
            options.HasIndex(ev => ev.InviteSenderId);
            
            options.Property(ev => ev.Token).HasMaxLength(1000).IsRequired(false);
            options.Property(ev => ev.Message).HasMaxLength(1500).IsRequired(false);
            options.Property(ev => ev.InvitationLink).HasMaxLength(1500).IsRequired(false);
            options.Property(ev => ev.RejectReason).HasMaxLength(1500).IsRequired(false);
            options.Property(ev => ev.SalaryRange).HasMaxLength(100).IsRequired();
            options.Property(ev => ev.EmploymentType).IsRequired();
            options.Property(ev => ev.ProposedStartDate).HasColumnType("TIMESTAMPTZ").IsRequired();
            options.Property(ev => ev.ProposedEndDate).HasColumnType("TIMESTAMPTZ").IsRequired(false);
            options.Property(ev => ev.Status).IsRequired();
            options.Property(ev => ev.Currency).IsRequired();
            options.Property(ev => ev.ResponseDate).HasColumnType("TIMESTAMPTZ").IsRequired(false);
            options.Property(ev => ev.ExpiresAt).HasColumnType("TIMESTAMPTZ").IsRequired();

            options.HasOne(ev => ev.User)
                .WithMany(ev => ev.InvitationsReceived)
                .HasForeignKey(ev => ev.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            options.HasOne(x => x.Position)
                .WithMany(x => x.EmployeeInvitations)
                .HasForeignKey(x => x.PositionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            options.HasOne(ev => ev.Enterprise)
                .WithMany(ev => ev.EmployeeInvitations)
                .HasForeignKey(ev => ev.EnterpriseId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            options.HasOne(ev => ev.InviteSender)
                .WithMany(ev => ev.InvitationsSent)
                .HasForeignKey(ev => ev.InviteSenderId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        });

        modelBuilder.Entity<PositionEntity>(options =>
        {
            options.ToTable("Positions");
            options.HasKey(ev => ev.Id);
            options.Property(ev => ev.Name).HasMaxLength(300).IsRequired();
            options.HasIndex(ev => ev.Name).IsUnique();
            options.HasIndex(ev => ev.IsActive);
            options.Property(x => x.Describe).HasMaxLength(600).IsRequired(false);
        });
        
        modelBuilder.Entity<FavoriteCommentEntity>(options =>
        {
            options.HasIndex(e => new { e.UserId, e.CommentId })
                .IsUnique();
           
            options.HasOne(e => e.Comment)
                .WithMany()
                .HasForeignKey(e => e.CommentId)
                .IsRequired();

            options.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        });
        
        modelBuilder.Entity<CommentBaseEntity>(options =>
        {
            options.ToTable("CommentBase");
            options.UseTptMappingStrategy();
            options.HasKey(x => x.Id);
            options.Property(x => x.Content).HasMaxLength(800).IsRequired();
            options.Property(x => x.IsActive).IsRequired();
            options.Property(x => x.ImageUrl).HasMaxLength(2048).IsRequired(false);
            options.Property(c => c.Depth).HasColumnType("SMALLINT").IsRequired(false);

            options.HasOne(x => x.ParentComment)
                .WithMany(x => x.Replies)
                .HasForeignKey(x => x.ParentCommentId)
                .IsRequired(false);

            options.HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .IsRequired(true);
        });

        modelBuilder.Entity<CommentPostEnterpriseEntity>(options =>
        {
            options.ToTable("CommentPostEnterprise");
            options.HasBaseType<CommentBaseEntity>();

            options.HasOne(x => x.Post)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.PostId)
                .IsRequired();
        });
        
        modelBuilder.Entity<CommentPostUserEntity>(options =>
        {
            options.ToTable("CommentPostUser");
            options.HasBaseType<CommentBaseEntity>();
            
            options.HasOne(x => x.Post)
                .WithMany(x => x.CommentPostUser)
                .HasForeignKey(x => x.PostId)
                .IsRequired();
        });
        
        modelBuilder.Entity<FavoritePostUserEntity>(options =>
        {
            options.ToTable("FavoritePostUser");
            options.HasKey(e => e.Id);
            
            options.HasIndex(e => e.UserId);
            options.HasIndex(e => e.PostUserId);

            options.HasOne(e => e.PostUser)
                .WithMany(e => e.FavoritePosts)
                .HasForeignKey(e => e.PostUserId)
                .IsRequired();

            options.HasOne(e => e.User)
                .WithMany(e => e.FavoritePosts)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            
            options.HasIndex(e => new { e.UserId, e.PostUserId })
                .IsUnique();
            
            options.Property(e => e.UserNotes).HasMaxLength(600).IsRequired(false);
            options.Property(e => e.UserRating).HasColumnType("SMALLINT").IsRequired(false);
        });
        
        modelBuilder.Entity<FavoritePostEnterpriseEntity>(options =>
        {
            options.ToTable("FavoritePostEnterprise");
            options.HasKey(e => e.Id);
            
            options.HasIndex(e => e.UserId);
            options.HasIndex(e => e.PostEnterpriseId);

            options.HasOne(e => e.PostEnterprise)
                .WithMany(e => e.FavoritePostsEnterprise)
                .HasForeignKey(e => e.PostEnterpriseId)
                .IsRequired();

            options.HasOne(e => e.User)
                .WithMany(e => e.FavoritePostsEnterprise)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            
            options.HasIndex(e => new { e.UserId, e.PostEnterpriseId })
                .IsUnique();
        });
        
        modelBuilder.Entity<UserSkillEntity>(options =>
        {
            options.HasKey(e => e.Id);
            options.HasIndex(e => e.UserId);
            options.HasIndex(e => e.SkillId);

            options.Property(e => e.YearsOfExperience).IsRequired(false);
            options.Property(e => e.ExternalCertificateUrl).IsRequired(false);
            options.Property(e => e.ProficiencyLevel).IsRequired(false);

            options.HasOne(e => e.Skill)
                .WithMany(e => e.UserSkill)
                .HasForeignKey(e => e.SkillId)
                .IsRequired();
            
            options.HasOne(e => e.User)
                .WithMany(e => e.UserSkill)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            
            options.HasIndex(e => new { e.UserId, e.SkillId })
                .IsUnique();
            
        });
        
        modelBuilder.Entity<SkillEntity>(options =>
        {
            options.ToTable("Skills");
            options.HasKey(s => s.Id);
            options.Property(s => s.Name).HasMaxLength(150).IsRequired();
            options.HasIndex(s => s.Name).IsUnique();
            options.Property(s => s.Description).HasMaxLength(500).IsRequired(false);
            options.Property(s => s.IconUrl).HasColumnType("TEXT").IsRequired(false);
            options.HasIndex(s => s.IsActive);
        });
        
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasOne(e => e.Enterprise)
                .WithOne(e => e.User);
        });

        modelBuilder.Entity<BasePostTable>(options =>
        {
            options.UseTptMappingStrategy();
            options.ToTable("PostsBase");
            options.Property(c => c.Title).HasMaxLength(500).IsRequired();
            options.Property(c => c.Content).HasColumnType("TEXT").IsRequired();
            options.Property(c => c.ImageUrl).HasColumnType("TEXT").IsRequired(false);
            options.Property(c => c.ReadingTimeMinutes).HasColumnType("SMALLINT").IsRequired(false);
        });

        modelBuilder.Entity<PostEnterpriseEntity>(options =>
        {
            options.HasBaseType<BasePostTable>();
            options.ToTable("PostEnterprises");
            options.HasOne(e => e.Enterprise)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.EnterpriseId)
                .IsRequired();
            
            options.HasOne(e => e.Category)
                .WithMany(e => e.PostsEnterprise)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired();
        });
        
        modelBuilder.Entity<PostUserEntity>(options =>
        {
            options.HasBaseType<BasePostTable>();
            options.ToTable("PostUsers");
            options.HasOne(e => e.User)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            
            options.HasOne(e => e.Category)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired();
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