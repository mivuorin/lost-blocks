using LostBlocks.Data;
using LostBlocks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Theme;

[ApiController]
[Route("theme")]
public class ThemeController(LegoContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<ThemeDto>> Get()
    {
        var themes = await context
            .Themes
            .AsNoTrackingWithIdentityResolution()
            .Include(theme => theme.Sets) // optimize to count
            .ToListAsync();

        var lookup = themes.ToLookup(t => t.ParentId);

        foreach (LegoTheme legoTheme in themes)
        {
            var children = lookup[legoTheme.Id];
            legoTheme.Childs = children.ToList();
        }

        var root = themes
            .Where(t => t.ParentId == null)
            .Select(MapTheme);

        return root;
    }

    private static ThemeDto MapTheme(LegoTheme theme)
    {
        return new ThemeDto
        {
            Id = theme.Id,
            Name = theme.Name,
            Sets = theme.Sets.Count + CountChildSets(theme.Childs),
            Themes = theme.Childs.Select(MapTheme).ToArray()
        };
    }

    private static int CountChildSets(IEnumerable<LegoTheme> children)
    {
        var count = 0;
        foreach (LegoTheme child in children)
        {
            count += child.Sets.Count + CountChildSets(child.Childs);
        }

        return count;
    }

    [HttpGet("{themeId}")]
    public async Task<ActionResult<ThemeDetailsDto>> GetById(int themeId)
    {
        ThemeDetailsDto? theme = await context
            .Themes
            .AsNoTrackingWithIdentityResolution()
            .Where(t => t.Id == themeId)
            .Select(t => new ThemeDetailsDto
            {
                Id = t.Id,
                Name = t.Name,
                ParentId = t.ParentId
            })
            .SingleOrDefaultAsync(t => t.Id == themeId);

        if (theme is null)
        {
            return NotFound();
        }

        return theme;
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateThemeDto themeDto)
    {
        var theme = new LegoTheme
        {
            Name = themeDto.Name,
            ParentId = themeDto.ParentId
        };

        context.Themes.Add(theme);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetById", new { theme.Id }, theme.Id);
    }

    [HttpPut("{themeId}")]
    public async Task<ActionResult> Put(int themeId, UpdateThemeDto themeDto)
    {
        LegoTheme? theme = await context.Themes.SingleOrDefaultAsync(t => t.Id == themeId);

        if (theme is null)
        {
            return NotFound();
        }

        theme.Name = themeDto.Name;
        theme.ParentId = themeDto.ParentId;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{themeId}")]
    public async Task<ActionResult> Delete(int themeId)
    {
        await context
            .Themes
            .Where(t => t.Id == themeId)
            .ExecuteDeleteAsync();

        return NoContent();
    }
}
