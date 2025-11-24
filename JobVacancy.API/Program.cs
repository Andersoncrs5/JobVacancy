using System.Text;
using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Repositories.Provider;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Services.Providers;
using JobVacancy.API.Utils.Facades;
using JobVacancy.API.Utils.Uow.Interfaces;
using JobVacancy.API.Utils.Uow.Provider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// POSTGRES
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("JWT is not configured");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseLazyLoadingProxies(); 
    options.UseNpgsql(
        connectionString,
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
        });
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = builder.Configuration["RedisConfiguration:ConnectionString"]
                 ?? throw new Exception("Redis connection string is missing.");

    return ConnectionMultiplexer.Connect(config);
});


builder.Services.AddScoped<IDatabase>(sp =>
{
    var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
    return multiplexer.GetDatabase();
});


// S3
var awsCredentials = new BasicAWSCredentials("minioadmin", "minioadminpassword");

var s3Config = new AmazonS3Config
{
    ServiceURL = "http://localhost:9000",
    ForcePathStyle = true,
    UseHttp = true, 
    RegionEndpoint = Amazon.RegionEndpoint.USEast1 
};

builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(awsCredentials, s3Config));

// JWT
IConfigurationSection jwtSettings = builder.Configuration.GetSection("jwt");
string? secretKey = jwtSettings.GetSection("SecretKey").Value;

if (string.IsNullOrEmpty(secretKey))
    throw new InvalidOperationException("JWT is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        c.SwaggerDoc(description.GroupName, new OpenApiInfo()
        {
            Title = $"Job Vacancy API {description.ApiVersion}", 
            Version = description.ApiVersion.ToString(),     
            Description = description.IsDeprecated ? "This API version has been deprecated." : "API for blog system with Swagger and PostgreSQL.",
            Contact = new OpenApiContact
            {
                Name = "Anderson",
                Email = "anderson.c.rms2005@gmail.com"
            }
        });
    }

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync($"Too Many Requests: {token}");
    };
    
    options.AddFixedWindowLimiter("authSystemPolicy", limit =>
    {
        limit.PermitLimit = 30;
        limit.Window = TimeSpan.FromSeconds(5);
        limit.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("CreateItemPolicy", limit =>
    {
        limit.PermitLimit = 40;
        limit.Window = TimeSpan.FromSeconds(5);
        limit.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("UpdateItemPolicy", limit =>
    {
        limit.PermitLimit = 30;
        limit.Window = TimeSpan.FromSeconds(5);
        limit.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("DeleteItemPolicy", limit =>
    {
        limit.PermitLimit = 40;
        limit.Window = TimeSpan.FromSeconds(5);
        limit.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("ListItemPolicy", limit =>
    {
        limit.PermitLimit = 60;
        limit.Window = TimeSpan.FromSeconds(10);
        limit.QueueLimit = 0;
    });
    options.AddFixedWindowLimiter("GetItemPolicy", limit =>
    {
        limit.PermitLimit = 60;
        limit.Window = TimeSpan.FromSeconds(10);
        limit.QueueLimit = 0;
    });

});

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// JUST REPOSITORIES
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IIndustryRepository, IndustryRepository>();
builder.Services.AddScoped<IEnterpriseRepository, EnterpriseRepository>();
builder.Services.AddScoped<IEnterpriseIndustryRepository, EnterpriseIndustryRepository>();
builder.Services.AddScoped<IPostUserRepository, PostUserRepository>();
builder.Services.AddScoped<IPostEnterpriseRepository, PostEnterpriseRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IUserSkillRepository, UserSkillRepository>();
builder.Services.AddScoped<IFavoritePostUserRepository, FavoritePostUserRepository>();
builder.Services.AddScoped<IFavoritePostEnterpriseRepository, FavoritePostEnterpriseRepository>();
builder.Services.AddScoped<ICommentPostUserRepository, CommentPostUserRepository>();
builder.Services.AddScoped<ICommentPostEnterpriseRepository, CommentPostEnterpriseRepository>();
builder.Services.AddScoped<IFavoriteCommentPostUserRepository, FavoriteCommentPostUserRepository>();
builder.Services.AddScoped<IFavoriteCommentPostEnterpriseRepository, FavoriteCommentPostEnterpriseRepository>();
builder.Services.AddScoped<IEmployeeInvitationRepository, EmployeeInvitationRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IEmployeeEnterpriseRepository, EmployeeEnterpriseRepository>();
builder.Services.AddScoped<IReviewEnterpriseRepository, ReviewEnterpriseRepository>();
builder.Services.AddScoped<IIndicationUserRepository, IndicationUserRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IVacancyRepository, VacancyRepository>();
builder.Services.AddScoped<IVacancySkillRepository, VacancySkillRepository>();
builder.Services.AddScoped<IApplicationVacancyRepository, ApplicationVacancyRepository>();
builder.Services.AddScoped<IFollowerRelationshipUserRepository, FollowerRelationshipUserRepository>();
builder.Services.AddScoped<IFollowerUserRelationshipEnterpriseRepository, FollowerUserRelationshipEnterpriseRepository>();
builder.Services.AddScoped<IEnterpriseFollowsUserRepository, EnterpriseFollowsUserRepository>();
builder.Services.AddScoped<IReviewUserRepository, ReviewUserRepository>();
builder.Services.AddScoped<IUserEvaluationRepository, UserEvaluationRepository>();
builder.Services.AddScoped<IUserContentReactionRepository, UserContentReactionRepository>();
builder.Services.AddScoped<IPostUserMetricsRepository, PostUserMetricsRepository>();
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();

// JUST SERVICES
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IIndustryService, IndustryService>();
builder.Services.AddScoped<IEnterpriseService, EnterpriseService>();
builder.Services.AddScoped<IEnterpriseIndustryService, EnterpriseIndustryService>();
builder.Services.AddScoped<IPostUserService, PostUserService>();
builder.Services.AddScoped<IPostEnterpriseService, PostEnterpriseService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IUserSkillService, UserSkillService>();
builder.Services.AddScoped<IFavoritePostUserService, FavoritePostUserService>();
builder.Services.AddScoped<IFavoritePostEnterpriseService, FavoritePostEnterpriseService>();
builder.Services.AddScoped<ICommentPostUserService, CommentPostUserService>();
builder.Services.AddScoped<ICommentPostEnterpriseService, CommentPostEnterpriseService>();
builder.Services.AddScoped<IFavoriteCommentPostUserService, FavoriteCommentPostUserService>();
builder.Services.AddScoped<IFavoriteCommentPostEnterpriseService, FavoriteCommentPostEnterpriseService>();
builder.Services.AddScoped<IEmployeeInvitationService, EmployeeInvitationService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IEmployeeEnterpriseService, EmployeeEnterpriseService>();
builder.Services.AddScoped<IReviewEnterpriseService, ReviewEnterpriseService>();
builder.Services.AddScoped<IIndicationUserService, IndicationUserService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IVacancyService, VacancyService>();
builder.Services.AddScoped<IVacancySkillService, VacancySkillService>();
builder.Services.AddScoped<IApplicationVacancyService, ApplicationVacancyService>();
builder.Services.AddScoped<IFollowerRelationshipUserService, FollowerRelationshipUserService>();
builder.Services.AddScoped<IFollowerUserRelationshipEnterpriseService, FollowerUserRelationshipEnterpriseService>();
builder.Services.AddScoped<IEnterpriseFollowsUserService, EnterpriseFollowsUserService>();
builder.Services.AddScoped<IReviewUserService, ReviewUserService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IUserEvaluationService, UserEvaluationService>();
builder.Services.AddScoped<IUserContentReactionService, UserContentReactionService>();
builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddScoped<IPostUserMetricsService, PostUserMetricsService>();
builder.Services.AddScoped<IResumeService, ResumeService>();

builder.Services.AddScoped<IMapperFacades, MapperFacades>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

WebApplication? app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); 

    var userManager = services.GetRequiredService<UserManager<UserEntity>>();
    var roleManager = services.GetRequiredService<RoleManager<RoleEntity>>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    
    var datasSystemSection = configuration.GetSection("DataSystem");
    
    string systemUserName = datasSystemSection["SystemName"] ?? throw new InvalidOperationException("System user name configuration is missing.");
    string systemUserEmail = datasSystemSection["systemUserEmail"] ?? throw new InvalidOperationException("System user email configuration is missing.");
    string systemUserPassword = datasSystemSection["SystemUserPassword"] ?? throw new InvalidOperationException("System user password configuration is missing.");

    var datasRoles = configuration.GetSection("Roles");
    string masterRole = datasRoles["MasterRole"] ?? throw new InvalidOperationException("Master role configuration is missing.");
    string userRole = datasRoles["UserRole"] ?? throw new InvalidOperationException("User role configuration is missing.");
    string superAdmRole = datasRoles["SuperAdmRole"] ?? throw new InvalidOperationException("Super adm role configuration is missing.");
    string enterpriseRole = datasRoles["EnterpriseRole"] ?? throw new InvalidOperationException("Enterprise role configuration is missing.");

    string[] roles = { userRole, masterRole, superAdmRole, enterpriseRole };
    
    foreach (string roleName in roles)
    {
        if (!roleManager.RoleExistsAsync(roleName).Result)
        {
            var role = new RoleEntity();
            role.Name = roleName;
            await roleManager.CreateAsync(role);
            Console.WriteLine($"Role '{roleName}' created.");
        }
        else
        {
            Console.WriteLine($"Role '{roleName}' already exists.");
        }
    }
    
    UserEntity? checkName = await userManager.FindByNameAsync(systemUserName);
    UserEntity? checkEmail = await userManager.FindByEmailAsync(systemUserEmail);
    UserEntity? systemUser;
    if (checkName == null &&  checkEmail == null)
    {
        try
        {
            systemUser = new UserEntity()
            {
                UserName = systemUserName,
                Email = systemUserEmail,
                EmailConfirmed = true,
            };
        
            var userCreated = await userManager.CreateAsync(systemUser, systemUserPassword);

            if (userCreated.Succeeded)
            {
                await context.SaveChangesAsync();

                var addRoleResult = await userManager.AddToRoleAsync(systemUser, masterRole);
                if (addRoleResult.Succeeded)
                {
                    await context.SaveChangesAsync();
                    Console.WriteLine($"User '{systemUserName}' added to role '{masterRole}'.");
                }
                else
                {
                    Console.WriteLine($"Error adding user '{systemUserName}' to role '{masterRole}': {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"Error creating user '{systemUserName}'.");
                foreach (var error in userCreated.Errors)
                {
                    Console.WriteLine($"Error creating user '{systemUserName}': {error.Description}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    else
    {
        var getUserCreated = await userManager.FindByNameAsync(systemUserName) ?? throw new InvalidOperationException("User creation failed.");
        
        if (!await userManager.IsInRoleAsync(getUserCreated, masterRole))
        {
            var addRoleResult = await userManager.AddToRoleAsync(getUserCreated, masterRole);
            if (addRoleResult.Succeeded)
            {
                await context.SaveChangesAsync();
                Console.WriteLine($"User '{systemUserName}' added to role '{masterRole}'.");
            }
            else
            {
                Console.WriteLine($"Error adding existing user '{systemUserName}' to role '{masterRole}': {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
            }
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseRateLimiter();
// app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }