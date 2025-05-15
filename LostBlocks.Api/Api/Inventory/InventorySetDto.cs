namespace LostBlocks.Api.Api.Inventory;

public record InventorySetDto
{
    public required string SetNum { get; init; }
    public required int Quantity { get; init; }
}
