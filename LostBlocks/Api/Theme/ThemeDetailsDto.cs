namespace LostBlocks.Api.Theme;

public record ThemeDetailsDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int? ParentId { get; init; }
}
