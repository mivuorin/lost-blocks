using FluentAssertions;
using LostBlocks.Api.InventorySet;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LostBlocks.Test.Api.InventorySet;

public class InventorySetControllerTest : DatabaseTest
{
    private readonly InventorySetController controller;

    public InventorySetControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new InventorySetController(Context);
    }

    [Theory]
    [LegoAutoData]
    public async Task Post_adds_set_to_inventory(LegoInventory inventory, LegoSet set, int quantity)
    {
        inventory.SetNum = "not null"; // TODO Fix SetNum relation

        Context.Inventories.Add(inventory);
        Context.Sets.Add(set);
        Context.SaveChanges();

        var inventorySetDto = new CreateInventorySetDto
        {
            InventoryId = inventory.Id,
            SetNum = set.SetNum,
            Quantity = quantity
        };

        ActionResult result = await controller.Post(inventorySetDto);
        result.Should().BeOfType<NoContentResult>();

        LegoInventorySet actual =
            Context.InventorySets.Single(inventorySet => inventorySet.InventoryId == inventory.Id);

        actual.SetNum.Should().Be(set.SetNum);
        actual.Quantity.Should().Be(quantity);
    }
}
