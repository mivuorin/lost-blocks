using FluentAssertions;
using LostBlocks.Api.Models;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoPartCategoryTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Fact]
    public void Insert_with_generated_id()
    {
        var category = new LegoPartCategory
        {
            Name = "Test Category"
        };

        Context.PartCategories.Add(category);
        Context.SaveChanges();

        category.Id.Should().NotBe(0);
        
        var actual = Context.PartCategories.Find(category.Id);
        actual.Should().NotBe(null);
    }
}
