using LostBlocks.Api.Data;
using LostBlocks.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Controllers;

[ApiController]
[Route("theme")]
public class ThemeController(LegoContext context)
{
    public async Task<IEnumerable<ThemeDto>> Get()
    {
        var themes = await context
            .Themes
            .AsNoTrackingWithIdentityResolution()
            .Include(theme => theme.Sets) // optimize to count
            .ToListAsync();

        var lookup = themes.ToLookup(t => t.ParentId);
        
        foreach (var legoTheme in themes)
        {
            legoTheme.Childs = lookup[legoTheme.Id].ToList();
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
            Sets = theme.Sets.Count,
            Themes = theme.Childs.Select(MapTheme).ToArray()
        };
    }
}
