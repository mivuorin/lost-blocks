namespace LostBlocks.Models;

public class LegoInventorySet
{
    public required int InventoryId { get; set; }
    public required string SetNum { get; set; }
    public required int Quantity { get; set; }
    public LegoInventory Inventory { get; set; } = null!;
    public LegoSet Set { get; set; } = null!;
}
