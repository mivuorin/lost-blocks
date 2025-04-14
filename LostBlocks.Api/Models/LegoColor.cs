namespace LostBlocks.Api.Models;

public class LegoColor
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    // TODO Use RGB bytes to represent RGB string
    public string Rgb { get; set; } = null!;
    public bool IsTransparent { get; set; }
}
