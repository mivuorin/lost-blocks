using LostBlocks.Api.Inventory;
using LostBlocks.Data;
using LostBlocks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Set;

[ApiController]
[Route("set")]
public class SetDetailsController(LegoContext context) : ControllerBase
{
    [HttpGet("{setNum}")]
    public async Task<ActionResult<SetDetailsDto>> Get(string setNum)
    {
        SetDetailsDto? found = await context
            .Sets
            .Where(s => s.SetNum == setNum)
            .Select(s => new SetDetailsDto
            {
                Name = s.Name,
                Year = s.Year,
                NumParts = s.NumParts,
                Inventories = s
                    .Inventories
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

    [HttpPost]
    public async Task<ActionResult> Post(CreateSetDto setDto)
    {
        var set = new LegoSet
        {
            SetNum = setDto.SetNum,
            Name = setDto.Name,
            ThemeId = setDto.ThemeId,
            Year = setDto.Year,
            NumParts = 0
        };

        context.Sets.Add(set);
        await context.SaveChangesAsync();

        return CreatedAtAction("Get", new { setNum = set.SetNum }, set.SetNum);
    }

    [HttpPut("{setNum}")]
    public async Task<ActionResult> Put(string setNum, UpdateSetDto setDto)
    {
        LegoSet? set = await context.Sets.SingleOrDefaultAsync(s => s.SetNum == setNum);

        if (set is null)
        {
            return NotFound();
        }

        set.Name = setDto.Name;
        set.Year = setDto.Year;
        set.ThemeId = setDto.ThemeId;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{setNum}")]
    public async Task<ActionResult> Delete(string setNum)
    {
        LegoSet? set = await context.Sets.SingleOrDefaultAsync(s => s.SetNum == setNum);

        if (set is null)
        {
            return NotFound();
        }

        context.Remove(set);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
