using LostBlocks.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Api.Set;

[ApiController]
[Route("set")]
public class SetController(LegoContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<LegoSetDto>> Query([FromQuery] int themeId)
    {
        return await context
            .Sets
            .Where(s => s.ThemeId == themeId)
            .Select(s => new LegoSetDto
            {
                SetNum = s.SetNum,
                Name = s.Name,
                Year = s.Year,
                NumParts = s.NumParts
            })
            .ToArrayAsync();
    }
}
