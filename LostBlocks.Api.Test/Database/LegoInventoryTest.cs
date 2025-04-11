using FluentAssertions;
using LostBlocks.Api.Models;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoInventoryTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Fact]
    public void Insert_with_generated_id()
    {
        // TODO Relies on existing test data
        var set = Context.LegoSets.First();
        
        var inventory = new LegoInventory
        {
            Version = 1,
            SetNum = set.SetNum
        };

        Context.Inventories.Add(inventory);
        Context.SaveChanges();

        inventory.Id.Should().NotBe(0);
        
        var actual = Context.Inventories.Find(inventory.Id);
        actual.Should().NotBe(null);
    }
    
    // TODO Relation tgo Set tests
}
