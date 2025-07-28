namespace LostBlocks.Api.Set;

public record CreateSetDto
{
    public required string SetNum { get; init; }
    public required string Name { get; init; }
    public required int Year { get; init; }
    public required int ThemeId { get; init; }
}
