using LostBlocks.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Controllers;

[ApiController]
[Route("set")]
public class SetDetailsController(LegoContext context) : ControllerBase
{
    [HttpGet("{setNum}")]
    public async Task<ActionResult<LegoSetDetailsDto>> Get(string setNum)
    {
        LegoSetDetailsDto? found = await context.Sets
            .Where(s => s.SetNum == setNum)
            .Select(s => new LegoSetDetailsDto
            {
                Name = s.Name,
                Year = s.Year,
                NumParts = s.NumParts,
                Inventories = s.Inventories
                    .Select(i => new LegoInventoryDto
                    {
                        Id = i.Id,
                        Version = i.Version
                    })
                    .ToArray()
            })
            .SingleOrDefaultAsync();

        if (found is null)
        {
            return NotFound();
        }

        return found;
    }
}
