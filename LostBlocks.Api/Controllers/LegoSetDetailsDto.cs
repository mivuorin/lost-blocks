namespace LostBlocks.Api.Controllers;

public record LegoSetDetailsDto
{
    public required string Name { get; init; }
    public required int? Year { get; init; }
    public required int? NumParts { get; set; }
}
