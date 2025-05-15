namespace LostBlocks.Api.Part;

public record PartDto
{
    public required string PartNum { get; set; }
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
}
