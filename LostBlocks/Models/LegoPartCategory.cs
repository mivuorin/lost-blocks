namespace LostBlocks.Models;

public class LegoPartCategory
{
    public int Id { get; set; }
    public required string Name { get; set; } = null!;
    public ICollection<LegoPart> Parts { get; set; } = [];
}
