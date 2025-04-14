using FluentAssertions;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoColorTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Theory]
    [LegoAutoData]
    public void Rgb_is_generated_properly(LegoColor color)
    {
        color.Rgb.Should()
            .HaveLength(6);

        var bytes = color.Rgb.Chunk(2);
        bytes.Should().HaveCount(3)
            .And.AllSatisfy(b =>
                string.Concat(b).Should().MatchRegex("^[0-9A-F]{2}$"));
    }

    [Theory]
    [LegoAutoData]
    public void Insert_with_generated_id(LegoColor color)
    {
        Context.Colors.Add(color);
        Context.SaveChanges();

        color.Id.Should().NotBe(0);

        LegoColor? actual = Context.Colors.Find(color.Id);
        actual.Should().NotBe(null);
    }

    [Theory]
    [LegoAutoData]
    public void IsTransparent_is_mapped_correctly(LegoColor transparent, LegoColor opaque)
    {
        opaque.IsTransparent = false;
        transparent.IsTransparent = true;

        Context.Colors.AddRange(transparent, opaque);
        Context.SaveChanges();

        LegoColor actualOpaque = Context.Colors.Single(c => c.Id == opaque.Id && c.IsTransparent == false);
        actualOpaque.IsTransparent.Should().BeFalse();

        LegoColor actualTransparent = Context.Colors.Single(c => c.Id == transparent.Id && c.IsTransparent == true);
        actualTransparent.IsTransparent.Should().BeTrue();
    }
}
