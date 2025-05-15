using LostBlocks.Data;
using LostBlocks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostBlocks.Api.Category;

[ApiController]
[Route("category")]
public class CategoryController(LegoContext context) : ControllerBase
{
    [HttpGet]
    public Task<CategoryDto[]> Get()
    {
        return context
            .PartCategories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToArrayAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        CategoryDto? category = await context
            .PartCategories.Where(c => c.Id == id)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .SingleOrDefaultAsync();

        if (category is null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateCategoryDto categoryDto)
    {
        var category = new LegoPartCategory
        {
            Name = categoryDto.Name
        };

        context.PartCategories.Add(category);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetById", new { category.Id }, category.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, UpdateCategoryDto categoryDto)
    {
        LegoPartCategory? category = await context.PartCategories.SingleOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        category.Name = categoryDto.Name;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{categoryId:int}")]
    public async Task<ActionResult> Delete(int categoryId)
    {
        await context
            .PartCategories.Where(c => c.Id == categoryId)
            .ExecuteDeleteAsync();

        return NoContent();
    }
}
