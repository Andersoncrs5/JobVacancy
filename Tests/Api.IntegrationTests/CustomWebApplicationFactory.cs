using Microsoft.AspNetCore.Mvc.Testing;
using JobVacancy.API;
using JobVacancy.API.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                AppDbContext context = scopedServices.GetRequiredService<AppDbContext>();
                try
                {
                    context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occured while seeding the database. {0} ", e);
                    throw;
                }
            }
        });
    }
}