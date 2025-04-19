namespace LostBlocks.Api.Controllers;

public record LegoInventoryDto
{
    public required int Id { get; init; }
    public required int Version { get; init; }
}