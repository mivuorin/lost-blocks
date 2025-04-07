namespace LostBlocks.Api.Models;

public class LegoColor
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Rgb { get; set; } = null!;
    public char IsTrans { get; set; }
}
