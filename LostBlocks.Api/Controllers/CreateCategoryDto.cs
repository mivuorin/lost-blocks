namespace LostBlocks.Api.Controllers;

public record CreateCategoryDto
{
    public required string Name { get; init; }
}
