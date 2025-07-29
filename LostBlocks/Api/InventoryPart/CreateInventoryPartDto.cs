namespace LostBlocks.Api.InventoryPart;

public record CreateInventoryPartDto
{
    public required int InventoryId { get; set; }
    public required string PartNum { get; set; }
    public required int ColorId { get; set; }
    public required int Quantity { get; set; }
    public required bool IsSpare { get; set; }
}
