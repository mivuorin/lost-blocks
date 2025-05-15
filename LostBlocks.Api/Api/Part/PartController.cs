using LostBlocks.Api.Data;
using LostBlocks.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Api.Part;

[ApiController]
[Route("part")]
public class PartController(LegoContext context) : ControllerBase
{
    // TODO Parts by category & etc.
    [HttpGet("{partNum}")]
    public async Task<ActionResult<PartDto>> GetByPartNum(string partNum)
    {
        PartDto? part = await context
            .Parts
            .Where(p => p.PartNum == partNum)
            .Select(p => new PartDto
            {
                PartNum = p.PartNum,
                Name = p.Name,
                CategoryId = p.CategoryId
            })
            .SingleOrDefaultAsync();

        if (part is null)
        {
            return NotFound();
        }

        return part;
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreatePartDto partDto)
    {
        var part = new LegoPart
        {
            PartNum = partDto.PartNum,
            Name = partDto.Name,
            CategoryId = partDto.CategoryId
        };

        context.Parts.Add(part);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetByPartNum", new { part.PartNum }, part.PartNum);
    }

    [HttpPut("{partNum}")]
    public async Task<ActionResult> Put(string partNum, UpdatePartDto partDto)
    {
        LegoPart? part = await context.Parts.SingleOrDefaultAsync(p => p.PartNum == partNum);

        if (part is null)
        {
            return NotFound();
        }

        part.Name = partDto.Name;
        part.CategoryId = partDto.CategoryId;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{partNum}")]
    public async Task<ActionResult> Delete(string partNum)
    {
        await context
            .Parts.Where(p => p.PartNum == partNum)
            .ExecuteDeleteAsync();

        return NoContent();
    }
}
