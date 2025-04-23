namespace LostBlocks.Api.Models;

public class LegoInventoryPart
{
    public int InventoryId { get; set; }
    public string PartNum { get; set; } = null!;
    public int ColorId { get; set; }
    public int Quantity { get; set; }
    public bool IsSpare { get; set; }
    public required LegoInventory Inventory { get; set; }
    public required LegoColor Color { get; set; }
    public required LegoPart Part { get; set; }
}
