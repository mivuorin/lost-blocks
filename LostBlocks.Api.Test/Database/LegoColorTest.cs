using FluentAssertions;
using LostBlocks.Api.Models;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoColorTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Fact]
    public void Insert_with_generated_id()
    {
        var color = new LegoColor
        {
            Name = "new-color",
            IsTrans = 'f',
            Rgb = "0F0F0F",
        };

        Context.Colors.Add(color);
        Context.SaveChanges();

        color.Id.Should().NotBe(0);
        
        var actual = Context.Colors.Find(color.Id);
        actual.Should().NotBe(null);
    }
}
