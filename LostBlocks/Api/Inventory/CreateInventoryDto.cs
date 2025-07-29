namespace LostBlocks.Api.Inventory;

public record CreateInventoryDto
{
    public required int Version { get; set; }
    public required string SetNum { get; set; }
}
