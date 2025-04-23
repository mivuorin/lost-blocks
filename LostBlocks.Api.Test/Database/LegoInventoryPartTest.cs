using FluentAssertions;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoInventoryPartTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Theory]
    [LegoAutoData]
    public void Insert(LegoInventoryPart inventoryPart, LegoInventory inventory, LegoSet set, LegoColor color,
        LegoPart part)
    {
        inventory.Set = set;

        inventoryPart.Inventory = inventory;
        inventoryPart.Color = color;
        inventoryPart.Part = part;

        Context.InventoryParts.Add(inventoryPart);
        Context.SaveChanges();

        LegoInventoryPart actual = Context
            .InventoryParts
            .Include(ip => ip.Inventory)
            .Include(ip => ip.Color)
            .Include(ip => ip.Part)
            .Single(ip => ip.InventoryId == inventory.Id &&
                          ip.PartNum == part.PartNum &&
                          ip.ColorId == color.Id &&
                          ip.IsSpare == inventoryPart.IsSpare);

        actual.Should().NotBeNull();
        actual.Inventory.Should().Be(inventory);
        actual.Color.Should().Be(color);
        actual.Part.Should().Be(part);
    }
}
