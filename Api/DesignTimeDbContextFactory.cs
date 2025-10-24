using Api.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        
        var basePath = Directory.GetCurrentDirectory();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false) 
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada. Verifique o appsettings.json.");
        }
        
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        
        builder.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
        
        return new AppDbContext(builder.Options);
    }
}