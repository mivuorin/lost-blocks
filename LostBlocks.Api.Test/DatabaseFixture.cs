using LostBlocks.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace LostBlocks.Api.Test;

public class DatabaseFixture
{
    public DatabaseFixture()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", false, true)
            .Build();

        config.GetConnectionString("lego");

        var options = new DbContextOptionsBuilder<LegoContext>()
            .UseNpgsql(config.GetConnectionString("lego"))
            .EnableDetailedErrors() // Enable detailed errors for tests
            .Options;

        Factory = new PooledDbContextFactory<LegoContext>(options);
    }

    public PooledDbContextFactory<LegoContext> Factory { get; }
}
