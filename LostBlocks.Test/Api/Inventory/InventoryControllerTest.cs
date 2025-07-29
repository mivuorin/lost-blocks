using FluentAssertions;
using LostBlocks.Api.Inventory;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LostBlocks.Test.Api.Inventory;

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

    [Theory]
    [LegoAutoData]
    public async Task Post_saves_new_inventory(int version, string setNum)
    {
        var inventoryDto = new CreateInventoryDto
        {
            Version = version,
            SetNum = setNum
        };

        ActionResult result = await controller.Post(inventoryDto);
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = (CreatedAtActionResult)result;
        createdResult.Value.Should().BeOfType<int>();
        createdResult.ActionName.Should().Be(nameof(InventoryController.Get));

        var actualId = (int)createdResult.Value;

        LegoInventory actual = Context.Inventories.Single(i => i.Id == actualId);

        actual.Should().NotBeNull();

        actual.Version.Should().Be(version);
    }

    [Theory]
    [LegoAutoData]
    public async Task Delete_removes_inventory(LegoInventory inventory)
    {
        inventory.SetNum = "todo";

        Context.Inventories.Add(inventory);
        Context.SaveChanges();

        await controller.Delete(inventory.Id);

        LegoInventory? actual = Context.Inventories.SingleOrDefault(i => i.Id == inventory.Id);

        actual.Should().BeNull();
    }
}
