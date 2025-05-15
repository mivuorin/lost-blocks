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

        LegoPart actual = Context
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

    [Theory]
    [LegoAutoData]
    public void Delete_should_not_cascade(LegoPart part, LegoPartCategory category, LegoInventoryPart inventoryPart,
        LegoInventory inventory, LegoSet set, LegoColor color)
    {
        part.Category = category;

        inventory.Set = set;

        inventoryPart.Inventory = inventory;
        inventoryPart.Color = color;

        part.InventoryParts.Add(inventoryPart);

        Context.Parts.Add(part);
        Context.SaveChanges();

        var act = () => Context.Parts.Remove(part);
        act.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [LegoAutoData]
    public void Delete_when_part_is_not_used(LegoPart part, LegoPartCategory category)
    {
        part.Category = category;

        Context.Parts.Add(part);
        Context.SaveChanges();

        Context.Parts.Remove(part);
        Context.SaveChanges();

        LegoPart? actual = Context.Parts.AsNoTracking().SingleOrDefault(p => p.PartNum == part.PartNum);
        actual.Should().BeNull();
    }
}
