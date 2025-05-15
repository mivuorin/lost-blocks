namespace LostBlocks.Api.Theme;

public record ThemeDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Sets { get; init; }
    public required ThemeDto[] Themes { get; init; }
}
