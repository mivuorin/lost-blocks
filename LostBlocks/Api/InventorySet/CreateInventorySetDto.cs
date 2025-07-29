namespace LostBlocks.Api.InventorySet;

public record CreateInventorySetDto
{
    public required int InventoryId { get; init; }
    public required string SetNum { get; init; }
    public required int Quantity { get; set; }
}
