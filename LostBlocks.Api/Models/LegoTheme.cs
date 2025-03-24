namespace LostBlocks.Api.Models;

public class LegoTheme
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentId { get; set; }
}
