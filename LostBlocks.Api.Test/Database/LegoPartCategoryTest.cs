using FluentAssertions;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoPartCategoryTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Theory]
    [LegoAutoData]
    public void Insert_with_generated_id(LegoPartCategory category)
    {
        Context.PartCategories.Add(category);
        Context.SaveChanges();

        category.Id.Should().NotBe(0);

        LegoPartCategory? actual = Context.PartCategories.Find(category.Id);
        actual.Should().NotBe(null);
    }

    [Theory]
    [LegoAutoData]
    public void Has_many_LegoParts(LegoPartCategory category, LegoPart part1, LegoPart part2)
    {
        category.Parts.Add(part1);
        category.Parts.Add(part2);

        Context.PartCategories.Add(category);
        Context.SaveChanges();

        LegoPartCategory actual = Context
            .PartCategories.Include(pc => pc.Parts)
            .Single(pc => pc.Id == category.Id);

        actual
            .Parts
            .Should()
            .HaveCount(2)
            .And.Contain(part1)
            .And.Contain(part2);
    }
}
