using LostBlocks.Api.Data;
using LostBlocks.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Api.Theme;

[ApiController]
[Route("theme")]
public class ThemeController(LegoContext context) : ControllerBase
{
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
}
