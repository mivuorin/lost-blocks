namespace LostBlocks.Api.Models;

public class LegoSet
{
    public string SetNum { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int? Year { get; set; }
    public int? ThemeId { get; set; }
    public int? NumParts { get; set; }
    public LegoTheme? Theme { get; set; }
}
