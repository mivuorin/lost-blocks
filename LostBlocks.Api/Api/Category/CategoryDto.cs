namespace LostBlocks.Api.Api.Category;

public record CategoryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
