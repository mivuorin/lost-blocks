namespace LostBlocks.Api.Models;

public class LegoPart
{
    public required string PartNum { get; set; }
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
    public LegoPartCategory Category { get; set; } = null!;
    public ICollection<LegoInventoryPart> InventoryParts { get; set; } = [];
}
