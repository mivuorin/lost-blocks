using LostBlocks.Api.Data;
using LostBlocks.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Api.Color;

[Route("color")]
[ApiController]
public class ColorController(LegoContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<ColorDto>> Get()
    {
        return await context
            .Colors.Select(c => new ColorDto
            {
                Id = c.Id,
                Name = c.Name,
                Rgb = c.Rgb,
                IsTransparent = c.IsTransparent
            })
            .ToArrayAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ColorDto>> GetById(int id)
    {
        ColorDto? color = await context
            .Colors
            .Where(s => s.Id == id)
            .Select(c => new ColorDto
            {
                Id = c.Id,
                Name = c.Name,
                Rgb = c.Rgb,
                IsTransparent = c.IsTransparent
            })
            .SingleOrDefaultAsync();

        if (color is null)
        {
            return NotFound();
        }

        return color;
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateColorDto colorDto)
    {
        var color = new LegoColor
        {
            Name = colorDto.Name,
            Rgb = colorDto.Rgb,
            IsTransparent = colorDto.IsTransparent
        };

        context.Colors.Add(color);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetById", new { id = color.Id }, color.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, UpdateColorDto colorDto)
    {
        LegoColor? color = await context.Colors.FindAsync(id);

        if (color is null)
        {
            return NotFound();
        }

        color.IsTransparent = colorDto.IsTransparent;
        color.Name = colorDto.Name;
        color.Rgb = colorDto.Rgb;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await context
            .Colors
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

        return NoContent();
    }
}
