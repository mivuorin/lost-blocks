namespace LostBlocks.Api.Controllers;

public record InventorySetDto
{
    public required string SetNum { get; init; }
    public required int Quantity { get; init; }
}