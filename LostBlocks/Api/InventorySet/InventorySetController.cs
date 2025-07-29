using LostBlocks.Data;
using LostBlocks.Models;
using Microsoft.AspNetCore.Mvc;

namespace LostBlocks.Api.InventorySet;

[ApiController]
[Route("inventory-set")]
public class InventorySetController(LegoContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Post(CreateInventorySetDto inventorySetDto)
    {
        var inventorySet = new LegoInventorySet
        {
            InventoryId = inventorySetDto.InventoryId,
            SetNum = inventorySetDto.SetNum,
            Quantity = inventorySetDto.Quantity
        };

        context.InventorySets.Add(inventorySet);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
