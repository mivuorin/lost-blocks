namespace LostBlocks.Api.Controllers;

public record CreatePartDto
{
    public required string PartNum { get; set; }
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
}
