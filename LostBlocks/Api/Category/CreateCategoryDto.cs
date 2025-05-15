namespace LostBlocks.Api.Category;

public record CreateCategoryDto
{
    public required string Name { get; init; }
}
