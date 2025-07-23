using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SchedulerApi.Infrastructure.Services;
using SQLitePCL;

namespace SchedulerApi.Infrastructure.Persistence.Factories;

public class SchedulerDbContextFactory : IDesignTimeDbContextFactory<SchedulerDbContext>
{
    public SchedulerDbContext CreateDbContext(string[] args)
    {
        Console.WriteLine("[DEBUG] SchedulerDbContextFactory.CreateDbContext called");
        Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
        Console.WriteLine($"Files: {string.Join(", ", Directory.GetFiles(Directory.GetCurrentDirectory()))}");
        var envrionment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var envConnectionString = Environment.GetEnvironmentVariable("ASPNETCORE_SQLLITE_CS");
        Console.WriteLine(envConnectionString+envrionment);
        
        Batteries.Init();
        
        var connectionString = $"Data Source={envConnectionString}";

        var optionsBuilder = new DbContextOptionsBuilder<SchedulerDbContext>();
        optionsBuilder.UseSqlite(connectionString);
        var dummyTenantProvider = new DesignTimeTenantProvider();
        
        return new SchedulerDbContext(optionsBuilder.Options, dummyTenantProvider);
    }
    
    private class DesignTimeTenantProvider : ITenantProvider
    {
        public Guid GetCurrentUserId()
        {
            return Guid.Parse("00000000-0000-0000-0000-000000000001");
        }

        public bool GetIsProviderRequired()
        {
            return true;
        }

        Guid? ITenantProvider.GetCurrentUserId()
        {
            return GetCurrentUserId();
        }
    }
}