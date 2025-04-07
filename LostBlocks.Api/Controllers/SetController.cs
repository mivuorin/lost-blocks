using LostBlocks.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Controllers;

[ApiController]
[Route("set")]
public class SetController(LegoContext context)
{
    [HttpGet("{themeId}")]
    public async Task<IEnumerable<SetDto>> Get(int themeId)
    {
        return await context.LegoSets
            .Where(s => s.ThemeId == themeId)
            .Select(s => new SetDto
            {
                SetNum = s.SetNum,
                Name = s.Name,
                Year = s.Year,
                NumParts = s.NumParts
            })
            .ToArrayAsync();
    }
}
