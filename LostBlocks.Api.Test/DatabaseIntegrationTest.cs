using LostBlocks.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LostBlocks.Api.Test;

public class DatabaseIntegrationTest
{
    [Fact]
    public void Smoke()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", false, true)
            .Build();

        config.GetConnectionString("lego");

        var options = new DbContextOptionsBuilder<LegoContext>()
            .UseNpgsql(config.GetConnectionString("lego"))
            .Options;
        
        using var context = new LegoContext(options);

        var themes = context.Themes.Take(10).ToArray();
        
        Assert.NotEmpty(themes);
    }
}
