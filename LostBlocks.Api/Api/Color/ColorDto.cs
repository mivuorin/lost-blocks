namespace LostBlocks.Api.Api.Color;

public class ColorDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Rgb { get; init; }
    public required bool IsTransparent { get; init; }
}
