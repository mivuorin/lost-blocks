namespace LostBlocks.Models;

public class LegoInventory
{
    public int Id { get; set; }
    public required int Version { get; set; }
    public required string SetNum { get; set; } = null!;
    public ICollection<LegoInventoryPart> InventoryParts { get; set; } = [];
    public LegoSet Set { get; set; } = null!;
    public ICollection<LegoInventorySet> InventorySets { get; set; } = [];
}
