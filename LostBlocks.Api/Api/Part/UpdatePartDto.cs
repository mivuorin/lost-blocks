namespace LostBlocks.Api.Api.Part;

public record UpdatePartDto
{
    public required string Name { get; init; }
    public required int CategoryId { get; init; }
}
