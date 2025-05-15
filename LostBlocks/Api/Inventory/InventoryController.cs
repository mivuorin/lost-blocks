using System.Linq.Expressions;
using LostBlocks.Data;
using LostBlocks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Inventory;

[ApiController]
[Route("inventory")]
public class InventoryController(LegoContext context) : ControllerBase
{
    [HttpGet("{inventoryId:int}")]
    public async Task<ActionResult<InventoryDetailsDto>> Get(int inventoryId)
    {
        InventoryDetailsDto? found = await context
            .Inventories
            .Where(i => i.Id == inventoryId)
            .Select(i => new InventoryDetailsDto
            {
                Id = i.Id,
                SetNum = i.SetNum,
                Version = i.Version,
                Sets = i
                    .InventorySets
                    .Select(s => new InventorySetDto
                    {
                        SetNum = s.SetNum,
                        Quantity = s.Quantity
                    })
                    .ToArray(),
                Parts = i
                    .InventoryParts
                    .AsQueryable()
                    .Where(p => p.IsSpare == false)
                    .Select(InventoryPartDto())
                    .ToArray(),
                Spares = i
                    .InventoryParts
                    .AsQueryable()
                    .Where(p => p.IsSpare)
                    .Select(InventoryPartDto())
                    .ToArray()
            })
            .SingleOrDefaultAsync();

        if (found is null)
        {
            return NotFound();
        }

        return found;
    }

    private static Expression<Func<LegoInventoryPart, InventoryPartDto>> InventoryPartDto()
    {
        return ip => new InventoryPartDto
        {
            Quantity = ip.Quantity,
            PartNum = ip.PartNum,
            Name = ip.Part.Name,
            Color = ip.Color.Name,
            Rgb = ip.Color.Rgb,
            Transparent = ip.Color.IsTransparent,
            Category = ip.Part.Category.Name
        };
    }
}
