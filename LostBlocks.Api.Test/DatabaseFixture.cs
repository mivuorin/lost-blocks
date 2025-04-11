using LostBlocks.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LostBlocks.Api.Test;

public class DatabaseFixture : IDisposable, IAsyncDisposable
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
        
        Context = new LegoContext(options);
    }

    public LegoContext Context { get; }

    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
