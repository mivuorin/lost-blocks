namespace LostBlocks.Api.Models;

public class LegoInventory
{
    public int Id { get; set; }
    public int Version { get; set; }
    public string SetNum { get; set; } = null!;
    public ICollection<LegoInventoryPart> InventoryParts { get; set; } = [];
    public required LegoSet Set { get; set; }
    public ICollection<LegoInventorySet> InventorySets { get; set; } = [];
}
