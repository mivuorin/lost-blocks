namespace LostBlocks.Api.Models;

public class LegoTheme
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; }
    public LegoTheme? Parent { get; set; }
    public ICollection<LegoTheme>  Childs { get; set; } = new List<LegoTheme>();
    public ICollection<LegoSet> Sets { get; set; } = new List<LegoSet>();
}
