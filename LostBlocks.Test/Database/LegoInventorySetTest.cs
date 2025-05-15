using FluentAssertions;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Test.Database;

public class LegoInventorySetTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Theory]
    [LegoAutoData]
    public void Insert(LegoInventorySet inventorySet, LegoInventory inventory, LegoSet parent, LegoSet child)
    {
        inventory.Set = parent;

        inventorySet.Inventory = inventory;
        inventorySet.Set = child;

        Context.InventorySets.Add(inventorySet);
        Context.SaveChanges();

        LegoInventorySet actual = Context
            .InventorySets
            .Include(i => i.Inventory)
            .Include(i => i.Set)
            .Single(i =>
                i.InventoryId == inventorySet.InventoryId &&
                i.SetNum == inventorySet.SetNum);

        actual.Should().NotBeNull();
        actual.Inventory.Should().Be(inventory);
        actual.Set.Should().Be(child);
    }
}
