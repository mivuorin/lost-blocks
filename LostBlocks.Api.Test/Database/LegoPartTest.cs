using FluentAssertions;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoPartTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Theory]
    [LegoAutoData]
    public void Insert(LegoPart part, LegoPartCategory category)
    {
        part.Category = category;

        Context.Parts.Add(part);
        Context.SaveChanges();

        LegoPart? actual = Context.Parts.Find(part.PartNum);
        actual.Should().NotBeNull();
    }

    [Theory]
    [LegoAutoData]
    public void Has_one_LegoCategory(LegoPart part, LegoPartCategory category)
    {
        part.Category = category;

        Context.Parts.Add(part);
        Context.SaveChanges();

        LegoPart? actual = Context
            .Parts
            .Include(p => p.Category)
            .Single(p => p.PartNum == part.PartNum);

        actual.Category.Should().Be(category);
    }

    [Theory]
    [LegoAutoData]
    public void Has_many_LegoInventoryParts(LegoPart part, LegoInventoryPart inventoryPart1,
        LegoInventoryPart inventoryPart2, LegoInventory inventory, LegoColor color, LegoSet set)
    {
        inventory.Set = set;

        inventoryPart1.Inventory = inventory;
        inventoryPart1.Color = color;

        inventoryPart2.Inventory = inventory;
        inventoryPart2.Color = color;

        part.InventoryParts.Add(inventoryPart1);
        part.InventoryParts.Add(inventoryPart2);

        Context.Parts.Add(part);
        Context.SaveChanges();

        LegoPart actual = Context
            .Parts
            .Include(p => p.InventoryParts)
            .Single(p => p.PartNum == part.PartNum);

        actual
            .InventoryParts
            .Should()
            .HaveCount(2)
            .And.Contain(inventoryPart1)
            .And.Contain(inventoryPart2);
    }
}
