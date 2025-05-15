namespace LostBlocks.Api.Api.Inventory;

public record InventoryPartDto
{
    public required string PartNum { get; init; }
    public required int Quantity { get; init; }
    public required string Name { get; init; }
    public required string Color { get; init; }
    public required string Rgb { get; init; }
    public required bool Transparent { get; init; }
    public required string Category { get; init; }
}
