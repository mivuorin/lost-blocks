using LostBlocks.Api.Data;
using LostBlocks.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Controllers;

[ApiController]
[Route("info")]
public class LegoInventoryInfo(LegoContext context) : ControllerBase
{
    [HttpGet]
    public List<LegoInventory> Get()
    {
        return context
            .Inventories
            .AsNoTracking()
            .ToList();
    }
}
