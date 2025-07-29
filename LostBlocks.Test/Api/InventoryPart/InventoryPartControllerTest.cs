using FluentAssertions;
using LostBlocks.Api.InventoryPart;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LostBlocks.Test.Api.InventoryPart;

public class InventoryPartControllerTest : DatabaseTest
{
    private readonly InventoryPartController controller;

    public InventoryPartControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new InventoryPartController(Context);
    }

    [Theory]
    [LegoAutoData]
    public async Task Post_adds_part_to_inventory(LegoInventory inventory, LegoColor color, string partNum,
        bool isSpare, int quantity)
    {
        inventory.SetNum = "not null"; // TODO Fix SetNum relation

        Context.Colors.Add(color);
        Context.Inventories.Add(inventory);
        Context.SaveChanges();

        var inventoryPartDto = new CreateInventoryPartDto
        {
            ColorId = color.Id,
            InventoryId = inventory.Id,
            PartNum = partNum,
            Quantity = quantity,
            IsSpare = isSpare
        };

        ActionResult result = await controller.Post(inventoryPartDto);
        result.Should().BeOfType<NoContentResult>();

        LegoInventoryPart actual = Context.InventoryParts.Single(ip => ip.InventoryId == inventory.Id);

        actual.PartNum.Should().Be(partNum);
        actual.ColorId.Should().Be(color.Id);
        actual.IsSpare.Should().Be(isSpare);
        actual.Quantity.Should().Be(quantity);
    }
}
