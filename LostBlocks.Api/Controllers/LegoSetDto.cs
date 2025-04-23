namespace LostBlocks.Api.Controllers;

public record LegoSetDto
{
    public required string SetNum { get; init; }
    public required string Name { get; init; }
    public required int? Year { get; init; }
    public required int? NumParts { get; init; }
}
