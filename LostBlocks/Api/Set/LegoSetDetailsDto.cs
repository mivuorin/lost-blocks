using LostBlocks.Api.Inventory;

namespace LostBlocks.Api.Set;

public record LegoSetDetailsDto
{
    public required string Name { get; init; }
    public required int? Year { get; init; }
    public required int? NumParts { get; init; }
    public required LegoInventoryDto[] Inventories { get; init; }
}
