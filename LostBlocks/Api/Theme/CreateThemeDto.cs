namespace LostBlocks.Api.Theme;

public record CreateThemeDto
{
    public required string Name { get; init; }
    public required int? ParentId { get; init; }
}
