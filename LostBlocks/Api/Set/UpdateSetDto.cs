namespace LostBlocks.Api.Set;

public record UpdateSetDto
{
    public required string Name { get; init; }
    public required int Year { get; init; }
    public required int ThemeId { get; init; }
}
