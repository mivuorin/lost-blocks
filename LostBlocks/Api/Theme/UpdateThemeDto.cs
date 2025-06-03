namespace LostBlocks.Api.Theme;

public record UpdateThemeDto
{
    public required string Name { get; init; }
    public required int? ParentId { get; init; }
}
