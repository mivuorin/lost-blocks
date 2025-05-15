namespace LostBlocks.Api.Controllers;

public record UpdatePartDto
{
    public required string Name { get; init; }
    public required int CategoryId { get; init; }
}
