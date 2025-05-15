namespace LostBlocks.Api.Inventory;

public record InventoryDetailsDto
{
    public required int Id { get; init; }
    public required string SetNum { get; init; }
    public required int Version { get; init; }
    public required InventorySetDto[] Sets { get; init; }
    public required InventoryPartDto[] Parts { get; init; }
    public required InventoryPartDto[] Spares { get; init; }
}
