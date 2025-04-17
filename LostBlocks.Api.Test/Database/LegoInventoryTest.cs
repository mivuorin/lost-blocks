using FluentAssertions;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoInventoryTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Theory]
    [LegoAutoData]
    public void Insert_with_generated_id(LegoInventory inventory, LegoSet set)
    {
        inventory.Set = set;

        Context.Inventories.Add(inventory);
        Context.SaveChanges();

        inventory.Id.Should().NotBe(0);

        LegoInventory? actual = Context.Inventories.Find(inventory.Id);
        actual.Should().NotBe(null);
    }

    [Theory]
    [LegoAutoData]
    public void Has_one_LegoSet(LegoInventory inventory, LegoSet set)
    {
        inventory.Set = set;

        Context.Inventories.Add(inventory);
        Context.SaveChanges();

        LegoInventory actual = Context.Inventories
            .Include(i => i.Set)
            .Single(i => i.Id == inventory.Id);

        actual.Set.Should().Be(set);
    }

    [Theory]
    [LegoAutoData]
    public void Has_many_LegoInventoryPart(LegoInventory inventory,
        LegoInventoryPart inventoryPart1,
        LegoInventoryPart inventoryPart2,
        LegoPart part,
        LegoColor color,
        LegoSet set)
    {
        inventory.Set = set;
        
        inventoryPart1.Part = part;
        inventoryPart1.Color = color;
        
        inventoryPart2.Part = part;
        inventoryPart2.Color = color;
        
        inventory.InventoryParts.Add(inventoryPart1);
        inventory.InventoryParts.Add(inventoryPart2);

        Context.Inventories.Add(inventory);
        Context.SaveChanges();

        LegoInventory actual = Context.Inventories
            .Include(i => i.InventoryParts)
            .Single(i => i.Id == inventory.Id);

        actual.InventoryParts.Should().HaveCount(2)
            .And.Contain(inventoryPart1)
            .And.Contain(inventoryPart2);
    }
}
