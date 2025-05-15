namespace LostBlocks.Models;

public class LegoColor
{
    public int Id { get; set; }
    public required string Name { get; set; }
    // TODO Use RGB bytes to represent RGB string
    public required string Rgb { get; set; }
    public bool IsTransparent { get; set; }
    public ICollection<LegoInventoryPart> InventoryParts { get; set; } = [];
}
