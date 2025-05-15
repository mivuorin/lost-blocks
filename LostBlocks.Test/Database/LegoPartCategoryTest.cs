using FluentAssertions;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Test.Database;

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

    [Theory]
    [LegoAutoData]
    public void Delete_should_not_cascade(LegoPartCategory category, LegoPart part)
    {
        category.Parts.Add(part);
        Context.PartCategories.Add(category);
        Context.SaveChanges();

        var act = () => Context.Remove(category);
        act.Should().Throw<InvalidOperationException>();
    }
}
