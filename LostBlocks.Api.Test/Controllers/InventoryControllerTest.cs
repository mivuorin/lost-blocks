using FluentAssertions;
using LostBlocks.Api.Controllers;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LostBlocks.Api.Test.Controllers;

public class InventoryControllerTest : DatabaseTest
{
    private readonly InventoryController controller;

    public InventoryControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new InventoryController(Context);
    }

    [Theory]
    [LegoAutoData]
    public async Task Get_by_inventory_id(LegoInventory inventory, LegoSet set, LegoInventorySet inventorySet,
        LegoSet child, LegoInventoryPart inventoryPart, LegoPart part, LegoPartCategory category, LegoColor color,
        LegoInventoryPart inventoryPartSpare, LegoPart spare, LegoPartCategory spareCategory)
    {
        inventory.Set = set;

        inventorySet.Set = child;
        inventory.InventorySets.Add(inventorySet);

        part.Category = category;
        
        inventoryPart.Part = part;
        inventoryPart.Color = color;
        inventoryPart.IsSpare = false;
        inventory.InventoryParts.Add(inventoryPart);

        spare.Category = spareCategory;
        
        inventoryPartSpare.Part = spare;
        inventoryPartSpare.Color = color;
        inventoryPartSpare.IsSpare = true;
        inventory.InventoryParts.Add(inventoryPartSpare);

        Context.Inventories.Add(inventory);
        Context.SaveChanges();

        var result = await controller.Get(inventory.Id);

        var expected = new InventoryDetailsDto
        {
            Id = inventory.Id,
            SetNum = inventory.SetNum,
            Version = inventory.Version,
            Sets =
            [
                new InventorySetDto
                {
                    SetNum = inventorySet.SetNum,
                    Quantity = inventorySet.Quantity
                }
            ],
            Parts =
            [
                new InventoryPartDto
                {
                    PartNum = inventoryPart.PartNum,
                    Name = part.Name,
                    Quantity = inventoryPart.Quantity,
                    Color = color.Name,
                    Rgb = color.Rgb,
                    Transparent = color.IsTransparent,
                    Category = category.Name
                }
            ],
            Spares =
            [
                new InventoryPartDto
                {
                    PartNum = inventoryPartSpare.PartNum,
                    Name = spare.Name,
                    Quantity = inventoryPartSpare.Quantity,
                    Color = color.Name,
                    Rgb = color.Rgb,
                    Transparent = color.IsTransparent,
                    Category = spareCategory.Name
                }
            ]
        };
        
        result.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Return_404_when_inventory_is_not_found()
    {
        var result = await controller.Get(-1);
        result.Result.Should().BeOfType<NotFoundResult>();
    }
}
