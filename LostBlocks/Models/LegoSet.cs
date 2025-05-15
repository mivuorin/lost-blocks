namespace LostBlocks.Models;

public class LegoSet
{
    public required string SetNum { get; set; }
    public required string Name { get; set; }
    public int? Year { get; set; }
    public int? ThemeId { get; set; }
    public int? NumParts { get; set; }
    public LegoTheme? Theme { get; set; }
    public ICollection<LegoInventory> Inventories { get; set; } = [];
    public ICollection<LegoInventorySet> InventorySets { get; set; } = [];
}
