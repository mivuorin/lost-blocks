namespace LostBlocks.Api.Controllers;

public record UpdateCategoryDto
{
    public required string Name { get; init; }
}
