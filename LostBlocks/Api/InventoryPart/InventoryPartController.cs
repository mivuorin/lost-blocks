using LostBlocks.Data;
using LostBlocks.Models;
using Microsoft.AspNetCore.Mvc;

namespace LostBlocks.Api.InventoryPart;

[ApiController]
[Route("inventory-part")]
public class InventoryPartController(LegoContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Post(CreateInventoryPartDto inventoryPartDto)
    {
        var inventoryPart = new LegoInventoryPart
        {
            InventoryId = inventoryPartDto.InventoryId,
            ColorId = inventoryPartDto.ColorId,
            PartNum = inventoryPartDto.PartNum,
            Quantity = inventoryPartDto.Quantity,
            IsSpare = inventoryPartDto.IsSpare
        };

        context.InventoryParts.Add(inventoryPart);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
