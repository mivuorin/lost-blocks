namespace LostBlocks.Api.Models;

public class LegoPart
{
    public required string PartNum { get; set; }
    public required string Name { get; set; }
    public int PartCatId { get; set; }
    public required LegoPartCategory Category { get; set; }
    public ICollection<LegoInventoryPart> InventoryParts { get; set; } = [];
}
